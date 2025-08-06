using System.Text;
using TaskManager.Enums;
using TaskManager.Managers;
using TaskManager.Models;

namespace TaskManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // encode for terminal
            Console.OutputEncoding = Encoding.UTF8;
            User user = new User();
            UserManager userManager = new UserManager();
            UserTask task = new UserTask();
            TaskList taskList = null!;
            

            string Choice;
            bool flag = false;
            do
            {

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("========================================");
                Console.WriteLine(" Welcome to TaskManager ");
                Console.WriteLine("========================================");
                Console.ResetColor();
                Console.WriteLine("Please select an option:");
                Console.WriteLine(" [1] Log In");
                Console.WriteLine(" [2] Sign Up");
                Console.WriteLine(" [3] End The Program");
                Console.Write("Your choice: ");
                Choice = Console.ReadLine() ?? "";
                bool flag2 = false ;
                switch (Choice)
                {
                    case "1":
                        {
                            Console.WriteLine("\n🔐 Please enter your login details.");
                            Console.Write("📧 Email: ");
                            string email = Console.ReadLine() ?? "";
                            Console.Write("🔑 Password: ");
                            string password = Console.ReadLine() ?? "";
                            user = userManager.Login(email, password) ?? null!;
                            if (user == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("❌ User not found. Please try again or sign up if you don't have an account.");
                                Console.ResetColor();

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\n✅ Welcome, {user.Name}! You have successfully logged in.");
                                Console.ResetColor();
                                taskList = new(user.Email);
                                flag = true;
                            }
                        }
                        break;
                    case "2":
                        {
                           
                            Console.WriteLine("\n📝 Please provide your details to sign up.");
                            Console.Write("👤 Name: ");
                            string name = Console.ReadLine() ?? "";
                            string email;
                            // do confirm  for the email 
                            do
                            {
                                Console.Write("📧 Email: ");
                                email = Console.ReadLine() ?? "";
                                if (email.Contains("@") && email.Contains("."))
                                    flag2 = true;
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("❌ Invalid email format. Please enter a valid email address.");
                                    Console.ResetColor();
                                  
                                }
                                       

                            }while(flag2 == false);
                            Console.Write("🔑 Password: ");
                            string password = Console.ReadLine() ?? "";
                            if(userManager.RegisterUser(name, email, password))
                            {

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("✅ Registration successful! You can now log in.");
                                Console.ResetColor();
                            }else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" ❌ User already exists. Please log in.");
                                Console.ResetColor();
                            }
                        }
                        break;
                    case "3":
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n👋 Good Bye! Thank you for using TaskManager.");
                            Console.ResetColor();
                            return;
                        }
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("⚠️ Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }

            } while (user == null || !flag);

            // clear the console

            flag = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n========================================");
                Console.WriteLine(" Task Actions Menu ");
                Console.WriteLine("========================================");
                Console.ResetColor();
                Console.WriteLine("Please select an action:");
                Console.WriteLine(" [1] ➕ Add Task");
                Console.WriteLine(" [2] ✏️ Edit Task");
                Console.WriteLine(" [3] 🗑️ Delete Task");
                Console.WriteLine(" [4] ✔️ Mark Task As Complete/Incomplete");
                Console.WriteLine(" [5] 📋 Show All Tasks");
                Console.WriteLine(" [6] 📂 Filter By Category");
                Console.WriteLine(" [7] 🎯 Filter By Priority");
                Console.WriteLine(" [8] 🚪 End The Program");
                Console.Write("Your choice: ");
                Choice =Console.ReadLine() ?? "";

                switch (Choice)
                {
                    case "1":
                        var addTask = new UserTask();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n📝 Let's add a new task!");
                        Console.ResetColor();
                        Console.Write("Enter Task Title: ");
                        addTask.Title = Console.ReadLine() ?? "";
                        Console.Write("Enter Task Description: ");
                        addTask.Description = Console.ReadLine() ?? "";
                        Console.Write("Enter Due Date (yyyy-MM-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
                            addTask.DueDate = dueDate;
                        else
                            addTask.DueDate = DateTime.Now.AddDays(1);
                        Console.Write("Enter Priority (Low, Medium, High): ");
                        if (Enum.TryParse<PriorityLevel>(Console.ReadLine(), true, out PriorityLevel priority))
                            addTask.Priority = priority;
                        else
                            addTask.Priority = PriorityLevel.Medium;
                        Console.Write("Enter Category (Study, Work, Personal, Health, Other): ");
                        if (Enum.TryParse<CategoryType>(Console.ReadLine(), true, out CategoryType category))
                            addTask.Category = category;
                        else
                            addTask.Category = CategoryType.Other;
                        addTask.IsCompleted = false;
                        addTask.UserEmail = user.Email;
                        if (taskList.AddTask(addTask))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("✅ Task added successfully!");
                            Console.ResetColor();

                        }
                        else
                            Console.WriteLine("❌ Task not added!");
                        break;
                    case "2":
                        
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n✏️ Edit an existing task.");
                        Console.ResetColor();
                        taskList.ShowAllTasks(user.Email);
                        UserTask updateTask = new UserTask();
                        Console.Write("Enter Task ID to edit: ");
                        int taskId = int.Parse(Console.ReadLine() ?? "");
                        updateTask = taskList.IsTaskExistById(taskId);
                        if (updateTask != null)
                        {
                            Console.Write("New Title (leave blank to keep current): ");
                            string newTitle = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(newTitle)) updateTask.Title = newTitle;
                            Console.Write("New Description (leave blank to keep current): ");
                            string newDesc = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(newDesc)) updateTask.Description = newDesc;
                            Console.Write("New Due Date (yyyy-MM-dd, leave blank to keep current): ");
                            string newDate = Console.ReadLine() ?? "";
                            if (DateTime.TryParse(newDate, out DateTime newDueDate)) updateTask.DueDate = newDueDate;
                            Console.Write("New Priority (Low, Medium, High, leave blank to keep current): ");
                            string newPriority = Console.ReadLine() ?? "";
                            if (Enum.TryParse<PriorityLevel>(newPriority, true, out PriorityLevel newPrio)) updateTask.Priority = newPrio;
                            Console.Write("New Category (Study, Work, Personal, Health, Other, leave blank to keep current): ");
                            string newCat = Console.ReadLine() ?? "";
                            if (Enum.TryParse<CategoryType>(newCat, true, out CategoryType newCategory)) updateTask.Category = newCategory;
                            taskList.UpdateTask(updateTask);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("✅ Task updated successfully!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("❌ Task not found.");
                            Console.ResetColor();
                        }
                        break;
                    case "3":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n🗑️ Delete a task.");
                        Console.ResetColor();
                        taskList.ShowAllTasks(user.Email);
                        Console.Write("Enter Task ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {

                           
                            if (taskList.RemoveTask(deleteId))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("✅ Task deleted successfully!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("❌ Can't Delete Task.");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("❌ Invalid Task ID.");
                            Console.ResetColor();
                        }
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n✔️ Mark a task as complete/incomplete.");
                        Console.ResetColor();
                        taskList.ShowAllTasks(user.Email);
                        Console.Write("Enter Task ID to toggle completion: ");
                        if (int.TryParse(Console.ReadLine(), out int completeId))
                        {
                            var completeTask = taskList.IsTaskExistById(completeId);
                            if (completeTask != null)
                            {
                                taskList.MarkAsComplete_InComlete(completeTask);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"✅ Task '{completeTask.Title}' marked as {(completeTask.IsCompleted ? "complete" : "incomplete")}!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("❌ Task not found.");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("❌ Invalid Task ID.");
                            Console.ResetColor();
                        }
                        break;
                    case "5":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\n📋 All Tasks:");
                        Console.ResetColor();
                        taskList.ShowAllTasks(user.Email);
                        break;
                    case "6":
                        taskList.ShowAllTasks(user.Email);
                        Console.WriteLine("Enter The Category: Study, Work, Personal, Health, Other");
                        if (Enum.TryParse<CategoryType>(Console.ReadLine() ?? "", ignoreCase: true, out CategoryType category1))
                        {
                            var filtered = taskList.FilterByCategory(category1);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"\n📂 Tasks in category '{category1}':");
                            Console.ResetColor();
                            if (filtered.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("No tasks found in this category.");
                                Console.ResetColor();
                            }
                            else
                            {
                                foreach (var t in filtered)
                                {
                                    Console.WriteLine($"- [{(t.IsCompleted ? "X" : " ")}] {t.Title} (Due: {t.DueDate:yyyy-MM-dd}, Priority: {t.Priority})");
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("❌ Invalid category! Valid options: Study, Work, Personal, Health, Other.");
                            Console.ResetColor();
                        }
                        break;
                    case "7":
                        taskList.ShowAllTasks(user.Email);
                        Console.WriteLine("Enter The Priority: Low, Medium, High");
                        if (Enum.TryParse<PriorityLevel>(Console.ReadLine() ?? "", ignoreCase: true, out PriorityLevel priority1))
                        {
                            var filtered = taskList.FilterByPriority(priority1);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"\n🎯 Tasks with priority '{priority1}':");
                            Console.ResetColor();
                            if (filtered.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("No tasks found with this priority.");
                                Console.ResetColor();
                            }
                            else
                            {
                                foreach (var t in filtered)
                                {
                                    Console.WriteLine($"- [{(t.IsCompleted ? "X" : " ")}] {t.Title} (Due: {t.DueDate:yyyy-MM-dd}, Category: {t.Category})");
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("❌ Invalid priority! Valid options: Low, Medium, High.");
                            Console.ResetColor();
                        }
                        break;
                    case "8":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n👋 Good Bye! Thank you for using TaskManager.");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("⚠️ Invalid Choice, please try again.");
                        Console.ResetColor();
                        break;
                }

            } while (true);


        }
    }
}
