using Micro_Account.Models;
using Micro_Person.Services.SQL;
using MySql.Data.MySqlClient;
using System.Data;

namespace Micro_Person.Services.Repository
{
    public class SQLStoreDbRepository : IRepository_Me_User
    {
        private string ConnectionString => @"server=localhost;port=3306;user=root;password=root;Pooling=true;database=PET_ACCOUNT_DB;";
        public SQLStoreDbRepository() => new ProceduresAndFunctions(ConnectionString);    //Auto create database to MySQL.
        
        public async Task CreateAccount(UserDTO user)
        {
            await ExecuteMySqlCommandAsync($@"CALL {nameof(Procedures.CreateAccount)}
                                                            ('{user.Id}', '{user.EmailAdress}', '{user.Telephone}',
                                                            '{user.Password}', '{user.FirstName}', '{user.SurName}',
                                                            '{user.Country}', '{user.City}','{user.Gender}','{user.DateOfBirthday.ToString("yyyy-MM-dd H:mm:ss")}'
                                                            );");                                                                                                                      
        }

        public async Task UpdateAccount(UserDTO user)
        {
            await ExecuteMySqlCommandAsync($@"CALL {nameof(Procedures.UpdateUserAccount)}
                                                            ('{user.Id}', @{nameof(PrimaryUserData.EmailAddress)}, @{nameof(PrimaryUserData.Telephone)},
                                                            '{user.Password}', '{user.FirstName}', '{user.SurName}',
                                                            '{user.Country}', '{user.City}','{user.Gender}', '{user.DateOfBirthday.ToString("yyyy-MM-dd H:mm:ss")}');",
                                                             () => new MySqlParameter[] {new($"@{nameof(PrimaryUserData.EmailAddress)}", user.EmailAdress),new($"@{nameof(PrimaryUserData.Telephone)}", user.Telephone) });
        }

        public async Task<DataSet> GetAccount(string Id) => await GetDataFromMySqlAsync($@"CALL {nameof(Procedures.GetAccount)} ('{Id}');");

        public async Task<DataSet> Login(string email, string password)
        {
            return await GetDataFromMySqlAsync($@"CALL {nameof(Procedures.Login)} ('{email}', '{password}');");
        }
        public async Task DeleteAccount(string Id) => await ExecuteMySqlCommandAsync($@"CALL {nameof(Procedures.DeleteAccount)} ('{Id}');");

        public async Task<DataSet> CheckingOnUnique(string Email , string Number)
        {
           return await GetDataFromMySqlAsync($@"START TRANSACTION; CALL {nameof(Procedures.CheckingMailOnUnique)} ('{Email}');
                                                       CALL {nameof(Procedures.CheckingNumberOnUnique)} ('{Number}'); COMMIT;");
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
