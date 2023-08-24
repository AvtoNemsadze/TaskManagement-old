using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }


        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskEntity task)
        {
            try
            {
                await _taskService.CreateTaskAsync(task);
                return Ok("Task Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the task.");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving tasks.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            if (!await _taskService.TaskExsistAsync(id))
            {
                return NotFound($"Task with Id '{id}' does not exsist");
            }

            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto updatedTask)
        {
            if (updatedTask == null)
            {
                return NotFound($"Task with Id '{id}' does not exsist");
            }

            try
            {
                var task = await _taskService.UpdateTaskAsync(id, updatedTask);

                return Ok(task);
            }
     
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, "An error occurred while updating the task.");
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            if (!await _taskService.TaskExsistAsync(taskId))
            {
                return NotFound($"Task with Id '{taskId}' does not exsist");
            }

            var taskEntity = await _taskService.GetTaskByIdAsync(taskId);

            if (taskEntity == null)
            {
                return NotFound();
            }

            _taskService.DeleteTaskAsync(taskEntity);
            await _taskService.SaveChangesAsync();

            return Ok("Task Deleted Successfully");
        }
    }
}
