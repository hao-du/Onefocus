using Microsoft.Extensions.Logging;
using Onefocus.Common.UnitOfWork;
using Onefocus.Search.Application.Interfaces.Repositories;
using Onefocus.Search.Application.Interfaces.UnitOfWork;
using Onefocus.Search.Infrastructure.Databases.DbContexts;

namespace Onefocus.Search.Infrastructure.UnitOfWork;

public class UnitOfWork(SearchDbContext context
        , ILogger<UnitOfWork> logger
        , ISearchIndexQueueRepository searchIndexQueueRepository
    ) : BaseUnitOfWork(context, logger), IUnitOfWork
{
    public ISearchIndexQueueRepository SearchIndexQueue { get; } = searchIndexQueueRepository;
}
