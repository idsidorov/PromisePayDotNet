using PromisePayDotNet.Dto;
using PromisePayDotNet.Internals;
using System.Threading.Tasks;

namespace PromisePayDotNet.Abstractions
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountByIdAsync(string bankAccountId);

        Task<BankAccount> CreateBankAccountAsync(BankAccount bankAccount);

        Task<bool> DeleteBankAccountAsync(string bankAccountId);

        Task<User> GetUserForBankAccountAsync(string bankAccountId);
    }
    public static class BankAccountRepositoryExtensions
    {
        public static BankAccount GetBankAccountById(this IBankAccountRepository repo, string bankAccountId)
        {
            return repo.GetBankAccountByIdAsync(bankAccountId).WrapResult();
        }
        public static BankAccount CreateBankAccount(this IBankAccountRepository repo, BankAccount bankAccount)
        {
            return repo.CreateBankAccountAsync(bankAccount).WrapResult();
        }
        public static bool DeleteBankAccount(this IBankAccountRepository repo, string bankAccountId)
        {
            return repo.DeleteBankAccountAsync(bankAccountId).WrapResult();
        }
        public static User GetUserForBankAccount(this IBankAccountRepository repo, string bankAccountId)
        {
            return repo.GetUserForBankAccountAsync(bankAccountId).WrapResult();
        }
    }
}
