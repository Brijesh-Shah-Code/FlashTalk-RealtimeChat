using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class UserRepo : IUserInterface
    {
        private readonly NpgsqlConnection _conn;

        public UserRepo(NpgsqlConnection conn)
        {
            _conn = conn;
        }


        public async Task<t_User> Login(string email, string password)
        {
            t_User user = null;

            await _conn.CloseAsync();
            await _conn.OpenAsync();

            var sql = "SELECT c_userid, c_username, c_email, c_password FROM t_user WHERE c_email = @email AND c_password = @password";

            using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                user = new t_User
                {
                    c_UserId = reader.GetInt32(0),
                    c_UserName = reader.GetString(1),
                    c_Email = reader.GetString(2),
                    c_Password = reader.GetString(3)
                };
            }

            return user;
        }


        public async Task<List<t_User>> GetAllUsers()
        {
            var users = new List<t_User>();
            await _conn.CloseAsync();
            await _conn.OpenAsync();

            var sql = "SELECT c_userid, c_username, c_email FROM t_user";
            using var cmd = new NpgsqlCommand(sql, _conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new t_User
                {
                    c_UserId = reader.GetInt32(0),
                    c_UserName = reader.GetString(1),
                    c_Email = reader.GetString(2)
                });
            }
            return users;
        }
    }
}