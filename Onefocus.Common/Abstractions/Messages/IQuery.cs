﻿using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messages;

public interface IQuery : IRequest<Result>, IBaseQuery
{
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery
{
}

public interface IBaseQuery
{
}