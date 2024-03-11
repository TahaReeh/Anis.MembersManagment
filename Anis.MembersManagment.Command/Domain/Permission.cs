namespace Anis.MembersManagment.Command.Domain
{
    public record Permission(
         bool Transfer,
         bool PurchaseCards,
         bool ManageDevices
    );
}
