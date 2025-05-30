﻿using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messages;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}