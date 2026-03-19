using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.Data;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
//using DotNetCoreSqlDb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;               // for SignOutAsync(...)
using Microsoft.AspNetCore.Authentication.Cookies;    // <-- Adjust if your DbContext lives in a different namespace
using DotNetCoreSqlDb.Models; 


namespace DotNetCoreSqlDb.Helpers
{
    public class LogHelper
    {
        private readonly MyDatabaseContext _context;

        public LogHelper(MyDatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task LogSignInAsync(
            Guid userId,
            string attemptedUsername
            )
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.ID == userId);

            if (user != null)
            {
                var log = new SignInLog
                {
                    UserId = userId,
                    UserName = attemptedUsername,
                    DateTime = DateTime.Now
                };
                _context.SignInLog.Add(log);

                await _context.SaveChangesAsync();
            }
        }

    }
}