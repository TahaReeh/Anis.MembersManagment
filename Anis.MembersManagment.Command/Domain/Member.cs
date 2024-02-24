using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Commands.AcceptInvitation;
using Anis.MembersManagment.Command.Commands.CancelInvitation;
using Anis.MembersManagment.Command.Commands.RejectInvitation;
using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Events;
using Anis.MembersManagment.Command.Extensions;
using Grpc.Core;

namespace Anis.MembersManagment.Command.Domain
{
    public class Member : Aggregate<Member>, IAggregate
    {
        public void SendInvitation(SendInvitationCommand command)
        {
            if (HasInvitationPending)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invitation still pending"));

            if (IsJoined)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The member already exists in this subscription"));

            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public void CancelInvitation(CancelInvitationCommand command)
        {
            ValidateRequest();
            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public void RejectInvitation(RejectInvitationCommand command)
        {
            ValidateRequest();
            ApplyNewChange(command.ToEvent(NextSequence));
        }
        public void AcceptInvitation(AcceptInvitationCommand command)
        {
            ValidateRequest();
            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public bool IsJoined { get; private set; }
        public bool HasInvitationPending { get; private set; }

        protected override void Mutate(Event @event)
        {
            switch (@event)
            {
                case InvitationSent e:
                    Mutate(e);
                    break;
                case InvitationAccepted e:
                    Mutate(e);
                    break;
                case InvitationRejected e:
                    Mutate(e);
                    break;
                case InvitationCancelled e:
                    Mutate(e);
                    break;
            }
        }

        public void Mutate(InvitationSent _)
        {
            IsJoined = false;
            HasInvitationPending = true;
        }

        public void Mutate(InvitationAccepted _)
        {
            IsJoined = true;
            HasInvitationPending = false;
        }

        public void Mutate(InvitationCancelled _)
        {
            IsJoined = false;
            HasInvitationPending = false;
        }

        public void Mutate(InvitationRejected _)
        {
            IsJoined = false;
            HasInvitationPending = false;
        }

        private void ValidateRequest()
        {
            if (IsJoined)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The member already exists in this subscription"));

            if (!HasInvitationPending && !IsJoined)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "This invitation is invalid"));
        }
    }
}
