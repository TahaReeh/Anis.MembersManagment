using Anis.MembersManagment.Query.EventHandlers.Cancelled;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using Azure.Messaging.ServiceBus;
using MediatR;
using System.Text;
using System.Text.Json;

namespace Anis.MembersManagment.Query.Infrastructure.ServiceBus
{
    public class MembersEventsListner : IHostedService
    {
        private ServiceBusSessionProcessor _processor;
        private readonly ServiceBusProcessor _deadLetterProcessor;
        private readonly ILogger<MembersEventsListner> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MembersEventsListner(MembersServiceBus serviceBus, IConfiguration configuration, ILogger<MembersEventsListner> logger,IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _processor = serviceBus.Client.CreateSessionProcessor(
                topicName: configuration["taha-member-managment"],
                subscriptionName: configuration["invitations-subscription"],
                options: new ServiceBusSessionProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 1,
                    MaxConcurrentSessions = 100,
                    MaxConcurrentCallsPerSession = 1,
                });

            _processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            _processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

            _deadLetterProcessor = serviceBus.Client.CreateProcessor(
                topicName: configuration["taha-member-managment"],
                subscriptionName: configuration["invitations-subscription"],
                options: new ServiceBusProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 10,
                    MaxConcurrentCalls = 10,
                    SubQueue = SubQueue.DeadLetter,
                });

            _deadLetterProcessor.ProcessMessageAsync += DeadLetterProcessor_ProcessMessageAsync;
            _deadLetterProcessor.ProcessErrorAsync += DeadLetterProcessor_ProcessErrorAsync;
        }

        private async Task Processor_ProcessMessageAsync(ProcessSessionMessageEventArgs arg) // TODO: convert to generic method to handle deadLetters
        {
            var json = Encoding.UTF8.GetString(arg.Message.Body);

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var isHandled = arg.Message.Subject switch
            {
                nameof(InvitationSent) => await mediator.Send(Deserialize<InvitationSent>(json)),
                nameof(InvitationCancelled) => await mediator.Send(Deserialize<InvitationCancelled>(json)),
                //nameof(InvitationAccepted) => await mediator.Send(Deserialize<InvitationAccepted>(json)),
                //nameof(InvitationRejected) => await mediator.Send(Deserialize<InvitationRejected>(json)),
                //nameof(MemberJoined) => await mediator.Send(Deserialize<MemberJoined>(json)),
                //nameof(MemberRemoved) => await mediator.Send(Deserialize<MemberRemoved>(json)),
                //nameof(MemberLeft) => await mediator.Send(Deserialize<MemberLeft>(json)),
                //nameof(PermissionChanged) => await mediator.Send(Deserialize<PermissionChanged>(json)),
                //_ => await mediator.Send(Deserialize<UnknownEvent>(json)), //should i add to invitation ??
                _ => throw new Exception("unknown exception")
            };

            if (isHandled)
            {
                await arg.CompleteMessageAsync(arg.Message);
            }
            else
            {
                _logger.LogWarning("Message {MessageId} not handled",arg.Message.MessageId);
                await Task.Delay(5000);
                await arg.AbandonMessageAsync(arg.Message);
            }
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
            var json = Encoding.UTF8.GetString(arg.Message.Body);

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var isHandled = arg.Message.Subject switch
            {
                nameof(InvitationSent) => await mediator.Send(Deserialize<InvitationSent>(json)),
                _ => throw new InvalidOperationException("unknown message type"),
            };

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

        public Task StartAsync(CancellationToken cancellationToken) => _processor.StartProcessingAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => _processor.CloseAsync(cancellationToken);

    }
}
