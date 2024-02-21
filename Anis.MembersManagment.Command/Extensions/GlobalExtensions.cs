using System.Security.Cryptography;
using System.Text;

namespace Anis.MembersManagment.Command.Extensions
{
    public static class GlobalExtensions
    {
        public static string CombineGuids(this Guid firstGuid, Guid secondGuid) =>
            $"{firstGuid}_{secondGuid}";
    }
}
