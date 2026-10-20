using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IUserInterface
    {
        Task<t_User> Login(string email, string password);

        Task<List<t_User>> GetAllUsers();
    }
}