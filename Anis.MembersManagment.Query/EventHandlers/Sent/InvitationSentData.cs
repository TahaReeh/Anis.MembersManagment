﻿using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.EventHandlers.Sent
{
    public record InvitationSentData(
    string AccountId,
    string SubscriptionId,
    string MemberId,
    Permission Permissions
    );

    //user_id why ????
}
