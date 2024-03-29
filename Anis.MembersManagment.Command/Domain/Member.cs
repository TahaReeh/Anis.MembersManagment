﻿using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Commands.AcceptInvitation;
using Anis.MembersManagment.Command.Commands.CancelInvitation;
using Anis.MembersManagment.Command.Commands.ChangePermission;
using Anis.MembersManagment.Command.Commands.JoinMember;
using Anis.MembersManagment.Command.Commands.Leave;
using Anis.MembersManagment.Command.Commands.RejectInvitation;
using Anis.MembersManagment.Command.Commands.RemoveMember;
using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Events;
using Anis.MembersManagment.Command.Exceptions;
using Anis.MembersManagment.Command.Extensions;

namespace Anis.MembersManagment.Command.Domain
{
    public class Member : Aggregate<Member>, IAggregate
    {
        #region Invitation
        public void SendInvitation(SendInvitationCommand command)
        {
            if (HasInvitationPending)
                throw new BusinessRuleViolationException("Invitation still pending");

            if (IsJoined)
                throw new AlreadyExistsException("The member already exists in this subscription");

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

        #endregion

        #region Member
        public void JoinMember(JoinMemberCommand command)
        {
            if (IsJoined)
                throw new AlreadyExistsException("The member already exists in this subscription");

            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public void RemoveMember(RemoveMemberCommand command)
        {
            if (!IsJoined)
                throw new NotFoundException("There is no such member in this subscription");

            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public void Leave(LeaveCommand command)
        {
            if (!IsJoined)
                throw new NotFoundException("There is no such member in this subscription");

            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public void ChangePermission(ChangePermissionCommand command)
        {
            if (!IsJoined)
                throw new NotFoundException("There is no such member in this subscription");

            if (HasInvitationPending)
                throw new BusinessRuleViolationException("Invitation still pending");

            if (command.Permissions == Permissions)
                throw new BusinessRuleViolationException("The member already has these permissions");

            ApplyNewChange(command.ToEvent(NextSequence));
        }

        #endregion

        public bool IsJoined { get; private set; }
        public bool HasInvitationPending { get; private set; }
        public Permission? Permissions { get; private set; }

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
                case MemberJoined e:
                    Mutate(e);
                    break;
                case MemberRemoved e:
                    Mutate(e);
                    break;
                case MemberLeft e:
                    Mutate(e);
                    break;
                case PermissionChanged e:
                    Mutate(e);
                    break;
            }
        }

        public void Mutate(InvitationSent @event)
        {
            IsJoined = false;
            HasInvitationPending = true;
            Permissions = @event.Data.Permissions;
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

        public void Mutate(MemberJoined @event)
        {
            IsJoined = true;
            HasInvitationPending = false;
            Permissions = @event.Data.Permissions;
        }

        public void Mutate(MemberRemoved _)
        {
            IsJoined = false;
            HasInvitationPending = false;
        }

        public void Mutate(MemberLeft _)
        {
            IsJoined = false;
            HasInvitationPending = false;
        }

        public void Mutate(PermissionChanged @event)
        {
            Permissions = @event.Data.Permissions;
        }

        private void ValidateRequest()
        {
            if (IsJoined)
                throw new AlreadyExistsException("The member already exists in this subscription");

            if (!HasInvitationPending && !IsJoined)
                throw new BusinessRuleViolationException("This invitation is invalid");
        }
    }
}
