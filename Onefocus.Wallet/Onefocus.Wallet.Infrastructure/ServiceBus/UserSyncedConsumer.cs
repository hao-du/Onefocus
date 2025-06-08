using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.ServiceBus
{
    internal class UserSyncedConsumer(
        IWriteUnitOfWork unitOfWork
            , ILogger<UserSyncedConsumer> logger
        ) : IConsumer<IUserSyncedMessage>
    {
        public async Task Consume(ConsumeContext<IUserSyncedMessage> context)
        {
            var getUserResult = await unitOfWork.User.GetUserByIdAsync(new(context.Message.Id));
            if (getUserResult.IsFailure)
            {
                ErrorLog(getUserResult, logger, context.Message);
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
                    ErrorLog(createUserResult, logger, context.Message);
                    return;
                }
                await unitOfWork.User.AddUserAsync(new(createUserResult.Value));
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
                    ErrorLog(updateUserResult, logger, context.Message);
                    return;
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        private static void ErrorLog(Result result, ILogger logger, IUserSyncedMessage message)
        {
            logger.LogError("Cannot insert or update user '{param1} {param2} - {param3} - {param4}' through message queue with [Code: {param5} Error: {param6}]",
               message.FirstName,
               message.LastName,
               message.Id,
               message.Email,
               result.Error.Code,
               result.Error.Description);
        }
    }
}
