using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Entity = Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Write;
using Microsoft.AspNetCore.Identity;

namespace Onefocus.Wallet.Application.User.ServiceBus
{
    internal class UserSyncedConsumer : IConsumer<IUserSyncedMessage>
    {
        private readonly IWriteUnitOfWork _unitOfWork;
        private readonly ILogger<UserSyncedConsumer> _logger;

        public UserSyncedConsumer(
            IWriteUnitOfWork unitOfWork
            , ILogger<UserSyncedConsumer> logger
        )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUserSyncedMessage> context)
        {
            var getUserResult = await _unitOfWork.User.GetUserByIdAsync(new(context.Message.Id));
            if (getUserResult.IsFailure)
            {
                ErrorLog(getUserResult, _logger, context.Message);
                return;
            }

            var user = getUserResult.Value.User;
            if (user == null)
            {
                var createUserResult = Entity.User.Create(
                        id: context.Message.Id,
                        email: context.Message.Email,
                        firstName: context.Message.FirstName,
                        lastName: context.Message.LastName,
                        description: context.Message.Description,
                        actionedBy: Guid.Empty
                    );
                if (createUserResult.IsFailure) 
                {
                    ErrorLog(createUserResult, _logger, context.Message);
                    return;
                }
                await _unitOfWork.User.AddUserAsync(new(createUserResult.Value));
            }
            else
            {
                var updateUserResult = user.Update(
                        email: context.Message.Email,
                        firstName: context.Message.FirstName,
                        lastName: context.Message.LastName,
                        description: context.Message.Description,
                        isActive: context.Message.IsActive,
                        actionedBy: Guid.Empty
                    );
                if (updateUserResult.IsFailure)
                {
                    ErrorLog(updateUserResult, _logger, context.Message);
                    return;
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private static void ErrorLog(Result result, ILogger logger, IUserSyncedMessage message)
        {
            logger.LogError($"Cannot insert or update user '{message.FirstName} {message.LastName} - {message.Id} - {message.Email}' through message queue with [Code: {result.Error.Code} Error: {result.Error.Description}]");
        }
    }
}
