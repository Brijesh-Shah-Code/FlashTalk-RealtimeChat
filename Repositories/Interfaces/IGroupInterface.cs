using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IGroupInterface
    {
        // Task JoinGroup(string groupName, string user);
        Task SendGroupMessage(string groupName, string sender, string message);
        Task<string> ReceiveGroupMessage(string groupName, string user);
    }
}