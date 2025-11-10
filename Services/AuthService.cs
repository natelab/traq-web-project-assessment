using Microsoft.EntityFrameworkCore;
using traq_web_project_assessment.Data;
using traq_web_project_assessment.Models;
using BCrypt.Net;

namespace traq_web_project_assessment.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
            {
                return null;
            }

            
            if (!VerifyPassword(password, user.PasswordHash)) // Verifying password using BCrypt
            {
                return null;
            }

            return user;
        }

        public async Task<bool> RegisterAsync(string username, string password, string fullName)
        {
            try
            {
                
                if (await UsernameExistsAsync(username))// Checking if the username does not already exists
                {
                    return false;
                }

                var user = new User
                {
                    Username = username,
                    PasswordHash = HashPassword(password),
                    FullName = fullName,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username);
        }

        public string HashPassword(string password)
        {
            // Using BCrypt to hash the password with a work factor of 12
            //I am using BCrypt to demonstrate a more secure form of hashing as compared to SHA256
            //Which would ideally be used in production
            // Work factor 12 for a good balance between security and performance
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                
                return BCrypt.Net.BCrypt.Verify(password, passwordHash); // Using BCrypt to verify the password against the hash
            }
            catch
            {
                // If the verification fails for example a wrong hash format then return false
                // If the verification fails for example a wrong hash format then return false
                return false;
            }
        }
    }
}