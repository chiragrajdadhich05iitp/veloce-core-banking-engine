using CoreLedger.Models;

namespace CoreLedger.Services
{
    public interface IAccountsService
    {
        Task CreateAsync(Account newAccount);
        Task<List<Account>> GetAsync();
        Task<Account?> GetAsync(string id);
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, Account updatedAccount);
    }
}