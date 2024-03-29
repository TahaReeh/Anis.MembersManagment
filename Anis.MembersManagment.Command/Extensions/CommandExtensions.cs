﻿using Anis.MembersManagment.Command.Commands.AcceptInvitation;
using Anis.MembersManagment.Command.Commands.CancelInvitation;
using Anis.MembersManagment.Command.Commands.ChangePermission;
using Anis.MembersManagment.Command.Commands.JoinMember;
using Anis.MembersManagment.Command.Commands.Leave;
using Anis.MembersManagment.Command.Commands.RejectInvitation;
using Anis.MembersManagment.Command.Commands.RemoveMember;
using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.MembersProto;

namespace Anis.MembersManagment.Command.Extensions
{
    public static class CommandExtensions
    {
        public static SendInvitationCommand ToCommand(this SendInvitationRequest request)
            => new() { 
                AccountId = request.AccountId ,
                SubscriptionId = request.SubscriptionId ,
                MemberId = request.MemberId ,
                UserId = request.UserId ,
                Permissions = new Permission
                (
                    Transfer : request.Permissions.Transfer,
                    PurchaseCards: request.Permissions.PurchaseCards,
                    ManageDevices: request.Permissions.ManageDevices
                )
            };

        public static AcceptInvitationCommand ToCommand(this AcceptInvitationRequest request)
           => new()
           {
               Id = request.Id,
               AccountId = request.AccountId,
               SubscriptionId = request.SubscriptionId,
               MemberId = request.MemberId,
               UserId = request.UserId
           };

        public static CancelInvitationCommand ToCommand(this CancelInvitationRequest request)
           => new()
           {
               Id = request.Id,
               AccountId = request.AccountId,
               SubscriptionId = request.SubscriptionId,
               MemberId = request.MemberId,
               UserId = request.UserId
           };

        public static RejectInvitationCommand ToCommand(this RejectInvitationRequest request)
            => new()
            {
                Id = request.Id,
                AccountId = request.AccountId,
                SubscriptionId = request.SubscriptionId,
                MemberId = request.MemberId,
                UserId = request.UserId
            };

        public static JoinMemberCommand ToCommand(this JoinMemberRequest request)
            => new()
            {
                AccountId = request.AccountId,
                SubscriptionId = request.SubscriptionId,
                MemberId = request.MemberId,
                UserId = request.UserId,
                Permissions = new Permission
                (
                    Transfer: request.Permissions.Transfer,
                    PurchaseCards: request.Permissions.PurchaseCards,
                    ManageDevices: request.Permissions.ManageDevices
                )
            };

        public static RemoveMemberCommand ToCommand(this RemoveMemberRequest request)
            => new()
            {
                Id = request.Id,
                AccountId = request.AccountId,
                SubscriptionId = request.SubscriptionId,
                MemberId = request.MemberId,
                UserId = request.UserId
            };

        public static LeaveCommand ToCommand(this LeaveRequest request)
            => new()
            {
                Id = request.Id,
                AccountId = request.AccountId,
                SubscriptionId = request.SubscriptionId,
                MemberId = request.MemberId,
                UserId = request.UserId,
            };

        public static ChangePermissionCommand ToCommand(this ChangePermissionRequest request)
           => new()
           {
               Id = request.Id,
               AccountId = request.AccountId,
               SubscriptionId = request.SubscriptionId,
               MemberId = request.MemberId,
               UserId = request.UserId,
               Permissions = new Permission
                (
                    Transfer: request.Permissions.Transfer,
                    PurchaseCards: request.Permissions.PurchaseCards,
                    ManageDevices: request.Permissions.ManageDevices
                )
           };
    }
}
