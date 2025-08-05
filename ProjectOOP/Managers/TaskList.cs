using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskManager.Enums;
using TaskManager.Models;
namespace TaskManager.Managers
{

    internal class TaskList
    {
        private List<UserTask> tasks;
        FileManager fileManager;
        private int lastTaskId;
        public TaskList(string useremail)
        {
            fileManager = new FileManager();
            tasks = fileManager.LoadTasksForUser(useremail);
            lastTaskId = tasks.Any() ? tasks.Max(t => t.Id) : 0;
        }

        #region AddTas;    
        public bool AddTask(UserTask task)
        {
            if (task != null)
            {
                lastTaskId++;       
                task.Id = lastTaskId;
                tasks.Add(task);
                fileManager.SaveTasksForUser(task.UserEmail, tasks);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region RemoveTask
        public bool RemoveTask(int id)
        {
            var deletedtask = tasks.FirstOrDefault(t => t.Id == id);
            if (deletedtask != null)
            {
                tasks.Remove(deletedtask);
                fileManager.SaveTasksForUser(deletedtask.UserEmail, tasks);
                return true;
            }
            return false;


        }

        #endregion


        #region Update
        public bool UpdateTask(UserTask task)
        {
            if (tasks.Any(t => t.Id == task.Id))
            {
                var index = tasks.FindIndex(t => t.Id == task.Id);
                tasks[index] = task;

                fileManager.SaveTasksForUser(task.UserEmail, tasks);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Completed
        public void MarkAsComplete_InComlete(UserTask task)
        {
            task.IsCompleted = task.IsCompleted == true ? false : true;
            if (tasks.Any(t => t.Id == task.Id))
            {
                tasks[tasks.FindIndex(t => t.Id == task.Id)] = task;
                fileManager.SaveTasksForUser(task.UserEmail, tasks);

                Console.WriteLine("Task marked as complete");
            }
            else
            {
                Console.WriteLine("Either the task is null or not in the list");
            }
        }
        #endregion


        #region AllTasks
        public void ShowAllTasks(string email)
        {
            //var showtasks = tasks.Where(e => e.UserEmail == email);
            foreach (var t in tasks)
            {

                Console.WriteLine(
                    $"   [{(t.IsCompleted == true ? "✔" : "✗")}]\n" +
                    $"   Id: {t.Id}\n" +
                    $"   Title: {t.Title}\n" +
                    $"   Description: {t.Description}\n" +
                    $"   Due: {t.DueDate:yyyy-MM-dd}\n" +
                    $"   Priority: {t.Priority}\n" +
                    $"   Category: {t.Category}\n" + $"\n"
                );
            }

        }
        #endregion

        #region Filters

        public List<UserTask> FilterByCategory(CategoryType category)
        =>
            tasks.Where(t => t.Category == category).ToList();



        public List<UserTask> FilterByPriority(PriorityLevel priority)
                        => tasks.Where(t => t.Priority == priority).ToList();


        #endregion


        #region TaskExist
        public UserTask? IsTaskExistById(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id) ?? null;
        }

        #endregion



    }


}
