using Onefocus.Common.UnitOfWork;
using Onefocus.Search.Application.Interfaces.Repositories;

namespace Onefocus.Search.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork : IBaseUnitOfWork
{
    ISearchIndexQueueRepository SearchIndexQueue { get; }
}