using Anis.MembersManagment.Query.EventHandlers.Accepted;
using Anis.MembersManagment.Query.EventHandlers.Cancelled;
using Anis.MembersManagment.Query.EventHandlers.Changed;
using Anis.MembersManagment.Query.EventHandlers.IncrementSequence;
using Anis.MembersManagment.Query.EventHandlers.Joined;
using Anis.MembersManagment.Query.EventHandlers.Left;
using Anis.MembersManagment.Query.EventHandlers.Rejected;
using Anis.MembersManagment.Query.EventHandlers.Removed;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using Azure.Messaging.ServiceBus;
using MediatR;
using System.Text;
using System.Text.Json;

namespace Anis.MembersManagment.Query.Infrastructure.ServiceBus
{
    public class MembersEventsListner : IHostedService
    {
        private readonly ServiceBusSessionProcessor _processor;
        private readonly ServiceBusProcessor _deadLetterProcessor;
        private readonly ILogger<MembersEventsListner> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MembersEventsListner(
            MembersServiceBus serviceBus,
            IConfiguration configuration,
            ILogger<MembersEventsListner> logger,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            var options = configuration.GetSection(ServiceBusOptions.ServiceBus).Get<ServiceBusOptions>();

            _processor = serviceBus.Client.CreateSessionProcessor(
                topicName: options!.TopicName,
                subscriptionName: options.SubscriptionName,
                options: new ServiceBusSessionProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 1,
                    MaxConcurrentSessions = 100,
                    MaxConcurrentCallsPerSession = 1,
                });

            _deadLetterProcessor = serviceBus.Client.CreateProcessor(
                topicName: options.TopicName,
                subscriptionName: options.SubscriptionName,
                options: new ServiceBusProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 10,
                    MaxConcurrentCalls = 10,
                    SubQueue = SubQueue.DeadLetter,
                });

            _processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            _processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

            _deadLetterProcessor.ProcessMessageAsync += DeadLetterProcessor_ProcessMessageAsync;
            _deadLetterProcessor.ProcessErrorAsync += DeadLetterProcessor_ProcessErrorAsync;
        }

        private async Task Processor_ProcessMessageAsync(ProcessSessionMessageEventArgs arg)
        {
            var isHandled = await TryHandleAsync(arg.Message);

            if (isHandled)
            {
                await arg.CompleteMessageAsync(arg.Message);
            }
            else
            {
                _logger.LogWarning("Message {MessageId} not handled", arg.Message.MessageId);
                await Task.Delay(5000);
                await arg.AbandonMessageAsync(arg.Message);
            }
        }


        private async Task<bool> TryHandleAsync(ServiceBusReceivedMessage message)
        {
            _logger.LogInformation(
                "Event {Event} Arrived, SessionId {SessionId}.",
                message.Subject,
                message.SessionId
            );

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var json = Encoding.UTF8.GetString(message.Body);

            return message.Subject switch
            {
                nameof(InvitationSent) => await mediator.Send(Deserialize<InvitationSent>(json)),
                nameof(InvitationCancelled) => await mediator.Send(Deserialize<InvitationCancelled>(json)),
                nameof(InvitationAccepted) => await mediator.Send(Deserialize<InvitationAccepted>(json)),
                nameof(InvitationRejected) => await mediator.Send(Deserialize<InvitationRejected>(json)),
                nameof(MemberJoined) => await mediator.Send(Deserialize<MemberJoined>(json)),
                nameof(MemberRemoved) => await mediator.Send(Deserialize<MemberRemoved>(json)),
                nameof(MemberLeft) => await mediator.Send(Deserialize<MemberLeft>(json)),
                nameof(PermissionChanged) => await mediator.Send(Deserialize<PermissionChanged>(json)),
                _ => await mediator.Send(Deserialize<UnknownEvent>(json))
            };
        }

        private static T Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
        ?? throw new InvalidOperationException("Faile to deserialize message");

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogCritical(arg.Exception, "Message handler encountered an exception," +
               " Error Source:{ErrorSource}," +
               " Entity Path:{EntityPath}",
               arg.ErrorSource.ToString(),
               arg.EntityPath
           );

            return Task.CompletedTask;
        }

        #region DeadLetter
        private Task DeadLetterProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogCritical(arg.Exception, "Message handler encountered an exception," +
               " Error Source:{ErrorSource}," +
               " Entity Path:{EntityPath}",
               arg.ErrorSource.ToString(),
               arg.EntityPath
           );

            return Task.CompletedTask;
        }

        private async Task DeadLetterProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var isHandled = await TryHandleAsync(arg.Message);

            if (isHandled)
            {
                await arg.CompleteMessageAsync(arg.Message);
            }
            else
            {
                _logger.LogWarning("Message {MessageId} not handled", arg.Message.MessageId);
                await Task.Delay(5000);
                await arg.AbandonMessageAsync(arg.Message);
            }
        }
        #endregion

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(
                _processor.StartProcessingAsync(cancellationToken),
                 _deadLetterProcessor.StartProcessingAsync(cancellationToken)
                );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(
                _processor.CloseAsync(cancellationToken),
                _deadLetterProcessor.CloseAsync(cancellationToken)
                );
        }

    }
}
