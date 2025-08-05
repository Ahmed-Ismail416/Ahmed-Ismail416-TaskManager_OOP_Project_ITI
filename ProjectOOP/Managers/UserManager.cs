using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Managers
{
    internal class UserManager
    {
        private List<User> users;
        
        private FileManager fileManager;
        public UserManager()
        {
            fileManager = new();
            users = fileManager.LoadUsers();
            
            
        }
       
        // register
        #region Register
        public bool RegisterUser(string name, string email, string password)
        {
            if (users.Any(e => e.Email == email))
            {
                return false;
            }
            else
            {
                var user = new User
                {
                    Name = name,
                    Email = email,
                    Password = password
                };
                try
                {
                    users.Add(user);
                    fileManager.SaveUsers(users);
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
        }
        #endregion

 

        #region Login
        public User? Login(string email, string password)
        {
            if (email == null || password == null || users.Any(e => e.Email == email) == false || users.Any(e => e.Password == password) == false)
                return null;
            else
                return users.FirstOrDefault(e => e.Email == email && e.Password == password);

        } 

        #endregion
    }
}
