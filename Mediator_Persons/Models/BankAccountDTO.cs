using Mediator_BankAccount.Services.SQL;
using System.Data;

namespace Mediator_BankAccount.Models
{
    public class BankAccountDTO
    {
        public BankAccountDTO(in int accountNumber, in double balance) => (AccountNumber, Balance) = (accountNumber, balance);
                            
        public BankAccountDTO(DataRow dataRow)
        {
            AccountNumber = Convert.ToInt32(dataRow[nameof(BankAccount.AccountNumber)]);
            Balance = Convert.ToDouble(dataRow[nameof(BankAccount.Balance)]);
        }

        public int AccountNumber { get; set; }
        public double Balance { get; set; }
    }
}
