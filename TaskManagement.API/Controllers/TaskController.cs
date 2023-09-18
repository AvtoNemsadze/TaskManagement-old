using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Enums;
using TaskManagement.API.Core.Hubs;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }


        [HttpPost]
        public async Task<IActionResult> CreateTask([FromForm] TaskCreateDto task, IFormFile? file)
        {
            try
            {
                await _taskService.CreateTaskAsync(task, file);
                return Ok("Task Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
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
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            if (!await _taskService.TaskExsistAsync(id))
            {
                return NotFound($"Task with Id '{id}' does not exsist");
            }

            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }


        [HttpGet("download/{fileUrl}")]
        public IActionResult DownloadFile(string fileUrl)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUrl);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); 
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            string contentType = GetContentType(fileUrl); 

            return File(fileBytes, contentType, Path.GetFileName(filePath));
        }

        private static string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream"; 
            }
            return contentType;
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
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                await _taskService.DeleteTaskAsync(taskId);
                return Ok("Task deleted successfully");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return NotFound($"Task with id '{taskId}' Not Found");
            }
        }

    }
}
