using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int taskId, int userId, string commentText)
        {
            var comment = await _commentService.CreateCommentAsync(taskId, userId, commentText);

            if (comment == null)
            {
                return BadRequest("Failed to create comment.");
            }

            return Ok(comment);
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult<Comment>> UpdateComment(int commentId, string text)
        {
            var updatedComment = await _commentService.UpdateCommentAsync(commentId, text);

            if (updatedComment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(updatedComment);
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentById(int commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment);
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByTaskId(int taskId)
        {
            var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }


        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            var result = await _commentService.DeleteCommentAsync(commentId);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return NotFound(result.Message); 
            }
        }

    }

}
