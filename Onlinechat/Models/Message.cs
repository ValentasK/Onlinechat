using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onlinechat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        public string MessageText { get; set; }
        public DateTime Created { get; set; }
    }
}
