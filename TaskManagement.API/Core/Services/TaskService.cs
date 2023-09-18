using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.DataAccess;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Hubs;
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

        public async Task<TaskEntity> CreateTaskAsync(TaskCreateDto taskCreateDto, IFormFile? file)
        {

            if (taskCreateDto == null)
            {
                throw new ArgumentNullException(nameof(taskCreateDto));
            }

            var taskStatus = TaskStatus.Started.ToString();

            // default deadline
            if (taskCreateDto.DueDate == null)
            {
                taskCreateDto.DueDate = DateTime.UtcNow.AddDays(7);
            }

            // add file
            string? fileToSave = null;

            if (file != null && file.Length > 0)
            {
                fileToSave = await SaveFileAsync(file);
            }

            // Check if the user exists
            var createdByUser = await _context.Users.FindAsync(taskCreateDto.CreatedByUserId) ?? throw new NullReferenceException("User not found");

            var newTask = new TaskEntity
            {
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                DueDate = taskCreateDto.DueDate,
                Status = taskStatus,
                Priority = taskCreateDto.Priority,
                UserId = taskCreateDto.UserId,
                TeamId = taskCreateDto.TeamId,
                AttachFile = fileToSave ?? null,
                CreatedByUserId = taskCreateDto.CreatedByUserId
            };

            newTask.CreatedByUserId = createdByUser.Id;

            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }

        private static async Task<string> SaveFileAsync(IFormFile? file)
        {
            string baseDirectory = "Documents";
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file?.FileName);
            string filePath = Path.Combine(baseDirectory, uniqueFileName); 

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; 
        }

        public async Task<IEnumerable<TaskGetDto>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks
               .Select(taskEntity => new TaskGetDto
               {
                   Id = taskEntity.Id,
                   Title = taskEntity.Title,
                   Description= taskEntity.Description,
                   DueDate = taskEntity.DueDate,
                   Status = taskEntity.Status,
                   Priority = taskEntity.Priority,
                   UserId = taskEntity.UserId ?? 0,
                   AttachFile = taskEntity.AttachFile,
               })
               .ToListAsync();

            return tasks;
        }

        public async Task<TaskGetDto> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.Tasks
               .Where(t => t.Id == taskId)
                .Select(taskEntity => new TaskGetDto
                {
                    Id = taskEntity.Id,
                    Title = taskEntity.Title,
                    Description = taskEntity.Description,
                    DueDate = taskEntity.DueDate,
                    Status = taskEntity.Status,
                    Priority = taskEntity.Priority,
                    UserId = taskEntity.UserId ?? 0,
                    AttachFile = taskEntity.AttachFile,
                })
               .FirstOrDefaultAsync();

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
            existingTask.UserId = updatedTask.UserId;

            await _context.SaveChangesAsync();

            return existingTask;
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId) ?? throw new NullReferenceException("Task not found");

            // Find all comments associated with the task then Remove the comments
            var taskComments = _context.Comments.Where(c => c.TaskId == taskId);
            _context.Comments.RemoveRange(taskComments);


            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
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
