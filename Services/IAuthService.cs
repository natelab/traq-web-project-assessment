using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public interface IAuthService
    {     
        Task<User?> AuthenticateAsync(string username, string password); //User authentication

        Task<bool> RegisterAsync(string username, string password, string fullName); //User registration

        Task<bool> UsernameExistsAsync(string username); //Does the username exist?

        string HashPassword(string password); //Hash the password

        bool VerifyPassword(string password, string passwordHash);
    }
}