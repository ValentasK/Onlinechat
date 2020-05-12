using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Onlinechat.Models;

namespace Onlinechat.Data
{
    public class OnlinechatContext : DbContext
    {
        public OnlinechatContext (DbContextOptions<OnlinechatContext> options)
            : base(options)
        {
        }

        public DbSet<Onlinechat.Models.User> Users { get; set; }
        public DbSet<Onlinechat.Models.Message> Messages { get; set; }
    }
}
