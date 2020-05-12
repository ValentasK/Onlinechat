using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onlinechat.Data;
using Onlinechat.Models;

namespace Onlinechat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomAPIController : ControllerBase
    {
        private readonly OnlinechatContext _context;

        public ChatRoomAPIController(OnlinechatContext context)
        {
            _context = context;
        }




        // Register new user
        // POST: api/ChatRoomAPI/userName
        [HttpPut("{userName}")]
        public async Task<IActionResult> CreateUser(string userName)
        {
            User user;

            bool userExist = _context.Users.Any(x => x.Name == userName
                              && x.Created.Date == DateTime.Now.Date);

            if (userExist)
            {
                return new ObjectResult(0);
            }
            else
            {
                user = new User
                {
                    Name = userName,
                    Created = DateTime.Now
                };


                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Message msg = new Message()
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    MessageText = "<<<-- " + user.Name + " -->>> Has Joined the chat",
                    Created = DateTime.Now
                };

                _context.Messages.Add(msg);
                await _context.SaveChangesAsync();
            }
            return new ObjectResult(user.Id);
        }


        // Create and save new message
        // POST: api/ChatRoomAPI/userName
       // [HttpPut("{userid}/{message}")]
        //public async Task<IActionResult> CreateMessage(int userid , string message)
        //{
        //    Message msg = new Message()
        //    {
        //        UserId = userid,
        //        MessageText = message,
        //        Created = DateTime.Now
        //    };

        //    _context.Messages.Add(msg);
        //    await _context.SaveChangesAsync();

        //    return new ObjectResult(msg.Id);
        //}

        // Create and save new message
        // POST: api/ChatRoomAPI/userName
       // [HttpPut]
        //public async Task<IActionResult> CreateMessage([FromQuery] int userid, [FromQuery] string message)
        //{

        //    User user = await _context.Users.FindAsync(userid);

        //    Message msg = new Message()
        //    {
        //        UserId = userid,
        //        UserName = user.Name,
        //        MessageText = message,
        //        Created = DateTime.Now
        //    };

        //    _context.Messages.Add(msg);
        //    await _context.SaveChangesAsync();

        //    return new ObjectResult(msg.Id);
        //}



        [HttpPost]
        public async Task<IActionResult> CreateMessage([Bind("userid,message")] tempMessage tmp )
        {

            User user = await _context.Users.FindAsync(tmp.userid);

            Message msg = new Message()
            {
                UserId = tmp.userid,
                UserName = user.Name,
                MessageText = tmp.message,
                Created = DateTime.Now
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            return new ObjectResult(msg.Id);
        }

        // Get messages for the user using his Id
        // GET: api/ChatRoomAPI
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(int Id)
        {
            User user = await _context.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            List<Message> msgList = new List<Message>();

            msgList = _context.Messages.Where(x => DateTime.Compare(x.Created, user.Created) > 0).ToList();

            return new ObjectResult(msgList);
        }



        // POST: api/ChatRoomAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //[Route("/api/ChatRoomAPI/userLoser")]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        // DELETE: api/ChatRoomAPI/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<User>> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
