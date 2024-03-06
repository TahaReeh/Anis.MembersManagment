using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.Test.Fakers.EntitiesFakers
{
    public class UserFaker : RecordFaker<User>
    {
        public UserFaker()
        {
            RuleFor(i => i.Id, faker => faker.Random.Guid().ToString());
            RuleFor(i => i.Name, faker => $"User{faker.Random.Int()}");
        }
    }
}
