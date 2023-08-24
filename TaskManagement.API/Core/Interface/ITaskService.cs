using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Interface
{
    public interface ITaskService
    {
        Task<TaskEntity> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);
        Task<TaskEntity> UpdateTaskAsync(int id, TaskUpdateDto updatedTask);
        void DeleteTaskAsync(TaskEntity taskEntity);
        Task<bool> TaskExsistAsync(int taskId);
        Task<bool> SaveChangesAsync();
    }
}
