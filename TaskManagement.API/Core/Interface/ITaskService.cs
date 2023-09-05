using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Interface
{
    public interface ITaskService
    {
        Task<TaskGetDto> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskGetDto>> GetAllTasksAsync();
        Task<TaskEntity> CreateTaskAsync(TaskCreateDto taskCreateDto, IFormFile file);
        Task<TaskEntity> UpdateTaskAsync(int id, TaskUpdateDto updatedTask);
        public Task DeleteTaskAsync(int taskId);
        Task<bool> TaskExsistAsync(int taskId);
        Task<bool> SaveChangesAsync();
    }
}
