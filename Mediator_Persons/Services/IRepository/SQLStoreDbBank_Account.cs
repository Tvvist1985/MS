using Mediator_BankAccount.Services.SQL;
using MySql.Data.MySqlClient;
using System.Data;

namespace Mediator_BankAccount.Services.IRepository
{
    public class SQLStoreDbBank_Account : IRepository_Bank_Account
    {
        private string ConnectionString => @"server=localhost;port=3306;user=root;password=root;Pooling=true;database=PET_BANK_ACCOUNT_DB;";
        public SQLStoreDbBank_Account() => new ProceduresAndFunctions(ConnectionString);

        public async Task CreateBankAccount(string id, int accountNumber)
        {
            await ExecuteMySqlCommandAsync($"CALL {nameof(Procedures.CreateBankAccount)} ('{id}', {accountNumber});");
        }
        public async Task<DataSet> GetBankAccount(string Id) => await GetDataFromMySqlAsync($@"CALL {nameof(Procedures.GetBankAccount)} ('{Id}');");

        public async Task ToUpAccount(string id, int sum)
        {
            await ExecuteMySqlCommandAsync($"CALL {nameof(Procedures.UpdateBalanse)} ('{id}', {sum});");
        }

        private async Task ExecuteMySqlCommandAsync(string requestString, Func<MySqlParameter[]> parameters = default)
        {
            await Task.Run(async () =>
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    MySqlCommand command = new MySqlCommand(requestString, connection);
                    if (parameters is not null) command.Parameters.AddRange(parameters.Invoke());
                    await command.ExecuteNonQueryAsync();
                }
            });
        }

        private async Task<DataSet> GetDataFromMySqlAsync(string requestString, Func<MySqlParameter[]> parameters = default)
        {
            DataSet ds = default;
            await Task.Run(() =>
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlDataAdapter d = new(requestString, connection);
                    if (parameters is not null)
                        d.SelectCommand.Parameters.AddRange(parameters.Invoke());

                    ds = new DataSet();
                    d.Fill(ds);
                }
            });
            return ds;
        }
    }
}
