using Micro_Account.Models;
using System.Data;

namespace Micro_Person.Services.Repository
{
    public interface IRepository_Me_User
    {
        public Task CreateAccount(UserDTO user);
        public Task UpdateAccount(UserDTO user);
        public Task<DataSet> GetAccount(string Id);
        public Task<DataSet> Login(string email, string password);
        public Task DeleteAccount(string Id);
        public Task<DataSet> CheckingOnUnique(string Email, string Number);
    }
}
