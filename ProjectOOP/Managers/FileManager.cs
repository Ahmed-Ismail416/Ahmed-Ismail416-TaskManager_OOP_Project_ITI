using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaskManager.Models;

namespace TaskManager.Managers
{
    public class FileManager
    {
        private readonly string _basePath;

        public FileManager()
        {
            _basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data");
            Directory.CreateDirectory(_basePath); // تأكد إن فولدر Data موجود
        }

        #region Helper
        private string SanitizeEmail(string email)
        {
            return email.Replace("@", "_at_").Replace(".", "_dot_");
        }
        #endregion

        #region Tasks Per User

        public void SaveTasksForUser(string email, List<UserTask> tasks)
        {
            string userFolder = Path.Combine(_basePath, SanitizeEmail(email));
            Directory.CreateDirectory(userFolder);

            string filePath = Path.Combine(userFolder, "tasks.json");

            try
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Tasks saved successfully for {email}.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Failed to save tasks for {email}: {ex.Message}");
                Console.ResetColor();
            }
        }

        public List<UserTask> LoadTasksForUser(string email)
        {
            string userFolder = Path.Combine(_basePath, SanitizeEmail(email));
            string filePath = Path.Combine(userFolder, "tasks.json");

            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<UserTask>();
                }

                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<UserTask>>(json) ?? new List<UserTask>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error loading tasks for {email}: {ex.Message}");
                Console.ResetColor();
                return new List<UserTask>();
            }
        }

        #endregion

        #region User List (Global)

        private readonly string _userListPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "users.json");

        public void SaveUsers(List<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_userListPath, json);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Users saved successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Failed to save users: " + ex.Message);
                Console.ResetColor();
            }
        }

        public List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(_userListPath)) return new List<User>();

                var json = File.ReadAllText(_userListPath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error loading users: " + ex.Message);
                Console.ResetColor();
                return new List<User>();
            }
        }

        #endregion
    }
}
