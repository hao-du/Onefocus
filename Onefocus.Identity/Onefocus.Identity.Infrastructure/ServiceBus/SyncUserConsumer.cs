using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Common.Security;
using Onefocus.Identity.Application.Interfaces.Repositories;
using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.ServiceBus
{
    internal class SyncUserConsumer(
        IUserRepository userRepository
            , IPasswordHasher<User> passwordHasher
            , IAuthenticationSettings authenticationSettings
            , ILogger<SyncUserConsumer> logger
        ) : IConsumer<ISyncUserMessage>
    {
        public async Task Consume(ConsumeContext<ISyncUserMessage> context)
        {
            var getUserResult = await userRepository.GetUserByEmailAsync(new(context.Message.Email), context.CancellationToken);
            if (getUserResult.IsFailure) LogError(getUserResult, context.Message);

            string? password = null;
            if (!string.IsNullOrEmpty(context.Message.EncryptedPassword))
            {
                password = await Cryptography.Decrypt(context.Message.EncryptedPassword, authenticationSettings.SymmetricSecurityKey);
            }

            var user = getUserResult.Value.User;

            if (user == null)
            {
                var createUserResult = User.Create(context.Message.Email, context.Message.Id);
                if (createUserResult.IsFailure) LogError(createUserResult, context.Message);

                var saveNewUserResult = await userRepository.CreateUserAsync(new(
                    User: createUserResult.Value,
                    Password: password
                ), context.CancellationToken);
                if (saveNewUserResult.IsFailure) LogError(saveNewUserResult, context.Message);
            }
            else
            {
                string? hashedPasword = null;
                if (!string.IsNullOrEmpty(password))
                {
                    hashedPasword = passwordHasher.HashPassword(user, password);
                }
                user.Update(context.Message.Email, hashedPasword);

                var updateUserResult = await userRepository.UpdateUserAsync(new(user), context.CancellationToken);
                if (updateUserResult.IsFailure) LogError(updateUserResult, context.Message);
            }

            logger.LogInformation("User: {Email} was synched successfully.", context.Message.Email);
        }

        private void LogError(Result result, ISyncUserMessage message)
        {
            foreach (var error in result.Errors)
            {
                logger.LogError("Error when synching User: {Email} with Code: {Code}, Description: {Description}", message.Email, error.Code, error.Description);
            }
            throw new InvalidOperationException($"Error when synching User: {message.Email} with Code: {result.Error.Code}, Description: {result.Error.Description}");
        }
    }
}
