using Anis.MembersManagment.Query.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Query.Test.Fakers.EntitiesFakers
{
    public class SubscriptionFaker : RecordFaker<Subscription>
    {
        public SubscriptionFaker() {
            RuleFor(i => i.Id, faker => faker.Random.Guid().ToString());
            RuleFor(i => i.UserId, faker => faker.Random.Guid().ToString());
            RuleFor(i => i.Description, faker => $"Sub{faker.Random.Int()}");
        }
    }
}
