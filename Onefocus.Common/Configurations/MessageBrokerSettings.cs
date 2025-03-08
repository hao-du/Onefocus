using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Configurations
{
    public interface IMessageBrokerSettings
    {
        const string SettingName = "MessageBroker";

        string Host { get; }
        string UserName { get; }
        string Password { get; }
        string InstanceId { get; }

    }

    public class MessageBrokerSettings : IMessageBrokerSettings
    {
        public string Host { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string InstanceId { get; init; } = string.Empty;
    }
}
