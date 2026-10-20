using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IMessageInterface
    {
        Task SendMessage(string toEmail, string message);
        Task<string> ReceiveMessage(string userEmail);
    }
}

