using Onefocus.Common.Search.Schema;

namespace Onefocus.Wallet.Application.Interfaces.Services.Search;

public interface ISchemaManagementService
{
    Task InitializeSchemaAsync();
}