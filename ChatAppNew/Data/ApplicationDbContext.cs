using System;
using System.Collections.Generic;
using System.Text;
using ChatAppNew.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatAppNew.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }
    }
}
