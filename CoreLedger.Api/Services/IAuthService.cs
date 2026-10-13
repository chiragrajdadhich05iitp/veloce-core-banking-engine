using CoreLedger.Models;

namespace CoreLedger.Services
{
    public interface IAuthService
    {
        Task<User?> Login(string email, string password);
    }
}