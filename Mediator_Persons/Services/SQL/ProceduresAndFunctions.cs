using MySql.Data.MySqlClient;

namespace Mediator_BankAccount.Services.SQL
{
    public struct ProceduresAndFunctions
    {
        private string AddTableToDB => @$"CREATE TABLE {nameof(BankAccount)}  
                                                  (
                                                     {nameof(BankAccount.Id)} VARCHAR(36)  NOT NULL,
                                                     {nameof(BankAccount.AccountNumber)} INT(8)  NOT NULL,
                                                     {nameof(BankAccount.Balance)} DECIMAL(11,2) NOT NULL,                                                                                                                                                           
                                                     CONSTRAINT _ID PRIMARY KEY({nameof(BankAccount.Id)}),
                                                     CONSTRAINT _AccountNumber UNIQUE({nameof(BankAccount.AccountNumber)})                                                     
                                                  );

                                           CREATE TABLE {nameof(ChangingAccount)}  
                                                  (
                                                     {nameof(ChangingAccount.Id)}  INT AUTO_INCREMENT NOT NULL,
                                                     {nameof(ChangingAccount.TimeChanging)}  DATETIME  NOT NULL,                    
                                                     {nameof(ChangingAccount.Value)} DECIMAL(5,2)   NOT NULL,
                                                     {nameof(ChangingAccount.BankAccountId)} VARCHAR(36)  NOT NULL,                                                     
                                                     CONSTRAINT _ID PRIMARY KEY({nameof(ChangingAccount.Id)}),  

                                                     CONSTRAINT _BankAccountId FOREIGN KEY ({nameof(ChangingAccount.BankAccountId)}) 
                                                            REFERENCES {nameof(BankAccount)} ({nameof(BankAccount.Id)}) ON DELETE CASCADE                                                                                                                   
                                                  );";

        #region // procedures
        private string CreateBankAccount => $@"CREATE PROCEDURE {nameof(Procedures.CreateBankAccount)}
                                                    ({nameof(BankAccount.Id)} VARCHAR(36),{nameof(BankAccount.AccountNumber)} INT(8))                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
                                                    BEGIN                                                         
                                                        INSERT INTO {nameof(BankAccount)}
                                                        ({nameof(BankAccount.Id)}, {nameof(BankAccount.AccountNumber)}, {nameof(BankAccount.Balance)})                                                            
                                                        VALUES 
                                                        ({nameof(BankAccount.Id)}, {nameof(BankAccount.AccountNumber)}, '0.0');  
                                                    END;";
        private string GetBankAccount => $@"CREATE PROCEDURE {nameof(Procedures.GetBankAccount)}
                                                    ({nameof(BankAccount.Id)} VARCHAR(36))                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
                                                    BEGIN                                                         
                                                         SELECT _ba.{nameof(BankAccount.AccountNumber)}, _ba.{nameof(BankAccount.Balance)} FROM {nameof(BankAccount)} AS _ba WHERE _ba.{nameof(BankAccount.Id)} = {nameof(BankAccount.Id)};                                             
                                                    END;";
        private string UpdateBalanse => $@"CREATE PROCEDURE {nameof(Procedures.UpdateBalanse)}
                                                    ({nameof(BankAccount.Id)} VARCHAR(36), {nameof(ChangingAccount.Value)} INT)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
                                                    BEGIN                                                         
                                                        UPDATE {nameof(BankAccount)} AS _ba SET  _ba.{nameof(BankAccount.Balance)} = 
                                                            _ba.{nameof(BankAccount.Balance)} + {nameof(ChangingAccount.Value)}
                                                            WHERE _ba.{nameof(BankAccount.Id)} = {nameof(BankAccount.Id)};
                                                    COMMIT;
                                                    END;";
        #endregion

        #region // Triggers
        private string TrackingBalanse => $@"CREATE TRIGGER {nameof(Triggers.TrackingBalanse)} AFTER UPDATE ON {nameof(BankAccount)}
                                                                FOR EACH ROW
                                                                BEGIN 
                                                                    INSERT INTO {nameof(ChangingAccount)} 
                                                                      ({nameof(ChangingAccount.BankAccountId)},
                                                                       {nameof(ChangingAccount.Value)},
                                                                       {nameof(ChangingAccount.TimeChanging)})
                                                                    VALUE (OLD.{nameof(BankAccount.Id)},
                                                                        NEW.{nameof(BankAccount.Balance)} - OLD.{nameof(BankAccount.Balance)},
                                                                        NOW());                                                                                                                                                                                                           
                                                                END;";       
        #endregion

        public ProceduresAndFunctions(in string connectionString)  // CREATE DATA BASE AND OTHE COMPONENTS
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                    connection.Open();
            }
            catch
            {
                using (MySqlConnection connection2 = new MySqlConnection("server=localhost;port=3306;user=root;password=root;Pooling=true;"))
                {
                    connection2.Open();

                    MySqlCommand command = new MySqlCommand("CREATE DATABASE PET_BANK_ACCOUNT_DB", connection2);
                    command.ExecuteNonQuery();
                }
                using (MySqlConnection connection2 = new MySqlConnection(connectionString))
                {
                    connection2.Open();

                    MySqlCommand command = new MySqlCommand(AddTableToDB, connection2);
                    command.ExecuteNonQuery();
                    command.CommandText = CreateBankAccount;
                    command.ExecuteNonQuery();
                    command.CommandText = GetBankAccount;
                    command.ExecuteNonQuery();
                    command.CommandText = TrackingBalanse;
                    command.ExecuteNonQuery();
                    command.CommandText = UpdateBalanse;
                    command.ExecuteNonQuery();
                                       
                }
            }
        }
    }
}
