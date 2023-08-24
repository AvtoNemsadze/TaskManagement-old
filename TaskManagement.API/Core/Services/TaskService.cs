using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;
using TaskStatus = TaskManagement.API.Core.Enums.TaskStatus;

namespace TaskManagement.API.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TaskEntity> CreateTaskAsync(TaskEntity newTask)
        {
            if (newTask == null)
            {
                throw new ArgumentNullException(nameof(newTask));
            }

            newTask.Status = TaskStatus.Started.ToString();

            // default deadline
            if(newTask.DueDate == null)
            {
                newTask.DueDate = DateTime.UtcNow.AddDays(7);
            }

            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            return task ?? throw new NullReferenceException();
        }

        public async Task<TaskEntity> UpdateTaskAsync(int id, TaskUpdateDto updatedTask)
        {
            var existingTask = await _context.Tasks.FindAsync(id) ?? throw new NullReferenceException();

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.Status = updatedTask.Status;
            existingTask.Priority = updatedTask.Priority;

            await _context.SaveChangesAsync();

            return existingTask;
        }

        public void DeleteTaskAsync(TaskEntity taskEntity)
        {
            _context.Tasks.Remove(taskEntity);
        }

        public async Task<bool> TaskExsistAsync(int taskId)
        {
            return await _context.Tasks.AnyAsync(c => c.Id == taskId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
