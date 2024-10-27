using System.Data;

namespace Mediator_BankAccount.Services.IRepository
{
    public interface IRepository_Bank_Account
    {
        public Task CreateBankAccount(string id, int accountNumber);
        public Task<DataSet> GetBankAccount(string Id);
        public Task ToUpAccount(string id, int summ);
    }
}
