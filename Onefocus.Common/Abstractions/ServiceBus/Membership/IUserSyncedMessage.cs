using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.ServiceBus.Membership
{
    public interface IUserSyncedMessage
    {
        Guid Id { get; } 
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
        string? Description { get; }
        bool IsActive { get; }
        string? EncryptedPassword { get; }
    }
}