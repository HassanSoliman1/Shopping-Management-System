using ECommerceAPI.CreateUserDtos;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController(ApplicationDbContext _dbContext) : ControllerBase
    {
        [HttpPost("submit-message")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SubmitMessage([FromForm] ContactDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sendById= User.FindFirstValue(ClaimTypes.NameIdentifier);

            var contactMessage = new ContactMessage()
            {
                SendById = sendById,
                Subject = dto.Subject,
                Message = dto.Message,
                SentAt = DateTime.UtcNow
            };

            await _dbContext.ContactMessages.AddAsync(contactMessage);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Message sent to admin successfully, he will reply soon" });

        }

        [HttpGet("get-all-messages")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = _dbContext.ContactMessages
                .OrderByDescending(m => m.SentAt);
                
            foreach (var message in messages)
            {
                if (!message.IsReadByAdmin)
                {
                    message.IsReadByAdmin = true;
                }
                    
            }

            await _dbContext.SaveChangesAsync();

            var responseMessages = _dbContext.ContactMessages
                .Include(m => m.Sender)
                .Select(m => new
                {
                    m.Id,
                    m.Subject,
                    m.Message,
                    SenderName = m.Sender.UserName,
                    SenderEmail = m.Sender.Email,
                    m.SentAt,
                    m.Reply,
                    m.ReplyedAt,
                    m.IsReadByAdmin,
                    m.IsReadByCustomer
                }).OrderByDescending(m => m.SentAt).ToList();

            return Ok(responseMessages);
        }

        [HttpGet("get-my-message")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyMessages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = _dbContext.ContactMessages
                .Where(m => m.SendById == userId)
                .OrderBy(m => m.SentAt);

            foreach (var message in messages)
            {
                if (!message.IsReadByCustomer && message.Reply != null) 
                {
                    message.IsReadByCustomer = true;
                }

            }

            await _dbContext.SaveChangesAsync();

            var responseMessages = _dbContext.ContactMessages
                .Select(m => new
                {
                    m.Id,
                    m.Subject,
                    m.Message,
                    m.SentAt,
                    m.Reply,
                    m.ReplyedAt,
                    m.IsReadByAdmin,
                    m.IsReadByCustomer
                }).ToList();

            return Ok(responseMessages);
        }

        [HttpPost("reply-on-message/{messageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReplyOnMessage(int messageId, [FromBody]string replyText)
        {
            var contactMessage = await _dbContext.ContactMessages.FindAsync(messageId);
            if (contactMessage == null)
                return NotFound(new { message = "Contact message not found" });

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            contactMessage.Reply = replyText;
            contactMessage.ReplyedAt = DateTime.UtcNow;
            contactMessage.RepliedById = adminId;

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Reply sent." });
        }


        [HttpDelete("remove-message/{messageId}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> RemoveMessage(int messageId)
        {
            var message = await _dbContext.ContactMessages.FindAsync(messageId);
            if (message == null)
                return NotFound(new { message = "Message not found!" });

            _dbContext.Remove(message);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Message removed successfully" });
        }


    }
}
