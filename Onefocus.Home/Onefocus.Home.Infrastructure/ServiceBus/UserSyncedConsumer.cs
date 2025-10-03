using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Infrastructure.ServiceBus
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
                LogError(getUserResult, context.Message);
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
                    LogError(createUserResult, context.Message);
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
                    LogError(updateUserResult, context.Message);
                }
            }

            var saveChangesResult = await unitOfWork.SaveChangesAsync(context.CancellationToken);
            if (saveChangesResult.IsFailure)
            {
                LogError(saveChangesResult, context.Message);
            }
        }

        private void LogError(Result result, IUserSyncedMessage message)
        {
            foreach (var error in result.Errors)
            {
                logger.LogError("Error when synching User: {Email} with Code: {Code}, Description: {Description}", message.Email, error.Code, error.Description);
            }
            throw new InvalidOperationException($"Error when synching User: {message.Email} with Code: {result.Error.Code}, Description: {result.Error.Description}");
        }
    }
}
