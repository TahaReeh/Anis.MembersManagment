namespace Anis.MembersManagment.Query.Entities
{
    public class Subscription
    {
        private Subscription(string id, string userId, string description)
        {
            Id = id;
            UserId = userId;
            Description = description;
        }
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string? Description { get; private set; }
    }
}
