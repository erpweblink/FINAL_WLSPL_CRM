using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WLSPL_CRM.Models;
using WLSPL_CRM_2.Models;
using WLSPL_CRM_2.repository;

namespace WLSPL_CRM_2.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserRegistrationRepo _repo;
        private readonly IchatRepo _chatRepo;

        public ChatController(IchatRepo chatRepo, IUserRegistrationRepo repo)
        {
            _chatRepo = chatRepo;
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            string currentUser = HttpContext.Session.GetString("userName");
            string senderId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(currentUser) || string.IsNullOrEmpty(senderId))
                return RedirectToAction("Login", "Account");

            var chatExists = await _chatRepo.Getchatbyid(senderId);
            var users = await _repo.UserListOrg("GetOrgData");

            // Reuse Message model to hold user data (if that's what you're doing)
            var userModels = users.Select(u => new Message
            {
                Id = u.ID,
                UserName = u.UserName
            }).ToList();

            ViewBag.CurrentUser = currentUser;
            ViewBag.CurrentUserId = senderId;

            return View(userModels); // Model: List<Message>
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] GetMessagesRequest model)
        {
            string senderId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrWhiteSpace(senderId) ||
                model == null ||
                string.IsNullOrWhiteSpace(model.PartnerId) ||
                string.IsNullOrWhiteSpace(model.Message))
            {
                return BadRequest("Invalid data or session expired.");
            }

            var message = new Message
            {
                SenderId = Convert.ToInt32(senderId),
                ReceiverId = Convert.ToInt32(model.PartnerId),
                MessageText = model.Message,
                Timestamp = DateTime.UtcNow
            };

            int result = await _chatRepo.submitchat(message, null);
            if (result != 1)
                return StatusCode(500, "Failed to send message.");

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetMessages([FromBody] GetMessagesRequest model)
        {
            string senderId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(model.PartnerId))
            {
                return BadRequest("Invalid user or partner ID.");
            }

            var messages = await _chatRepo.GetMessagesBetweenUsers(senderId, model.PartnerId);

            // Return only plain data to avoid cycles
            var safeMessages = messages.Select(m => new
            {
                m.Id,
                m.SenderId,
                m.ReceiverId,
                m.MessageText,
                Timestamp = m.Timestamp.ToString("o")
            });

            return Json(safeMessages);
        }
    }

    // Simple request model
    public class GetMessagesRequest
    {
        public string PartnerId { get; set; }
        public string Message { get; set; }
    }
}

