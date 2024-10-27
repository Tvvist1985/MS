using MySql.Data.MySqlClient;

namespace Micro_Person.Services.SQL
{
    public struct ProceduresAndFunctions
    {
        private string AddTableToDB => @$"CREATE TABLE {nameof(PrimaryUserData)}  
                                                  (
                                                     {nameof(PrimaryUserData.Id)} VARCHAR(36)  NOT NULL,
                                                     {nameof(PrimaryUserData.EmailAddress)} VARCHAR(255)  NOT NULL,
                                                     {nameof(PrimaryUserData.Telephone)} VARCHAR(12) NOT NULL,
                                                     {nameof(PrimaryUserData.Password)}  VARCHAR(100) NOT NULL ,                                                                                                       
                                                     CONSTRAINT _ID PRIMARY KEY({nameof(PrimaryUserData.Id)}),
                                                     CONSTRAINT _EmailAddress UNIQUE({nameof(PrimaryUserData.EmailAddress)}),
                                                     CONSTRAINT _Telephone UNIQUE ({nameof(PrimaryUserData.Telephone)})
                                                  );
                                           CREATE TABLE {nameof(SecondaryUserData)}  
                                                  (
                                                     {nameof(SecondaryUserData.Id)}  INT AUTO_INCREMENT NOT NULL,
                                                     {nameof(SecondaryUserData.FirstName)}  VARCHAR(20)  NOT NULL,                    
                                                     {nameof(SecondaryUserData.SurName)}  VARCHAR(40)  NOT NULL,
                                                     {nameof(SecondaryUserData.DateOfBirthday)}  DATE  NOT NULL,
                                                     {nameof(SecondaryUserData.Gender)}  VARCHAR(5)  NOT NULL,                                                   
                                                     {nameof(SecondaryUserData.Country)}  VARCHAR(55)  NOT NULL,
                                                     {nameof(SecondaryUserData.City)}  VARCHAR(55)  NOT NULL,
                                                     {nameof(SecondaryUserData.PrimaryDataId)}  VARCHAR(36)  NOT NULL,

                                                     CONSTRAINT _ID PRIMARY KEY({nameof(SecondaryUserData.Id)}),                                                     
                                                     CONSTRAINT _PrimaryDataId FOREIGN KEY ({nameof(SecondaryUserData.PrimaryDataId)}) REFERENCES {nameof(PrimaryUserData)} ({nameof(PrimaryUserData.Id)}) ON DELETE CASCADE,
                                                     CONSTRAINT _PrimaryDataIdUniq UNIQUE ({nameof(SecondaryUserData.PrimaryDataId)})                                                               
                                                  );";

        #region //Procedures
        private string CreateAccount => $@"CREATE PROCEDURE {nameof(Procedures.CreateAccount)}
                                                    ({nameof(PrimaryUserData.Id)} VARCHAR(36),
                                                    {nameof(PrimaryUserData.EmailAddress)} varchar(255),
                                                    {nameof(PrimaryUserData.Telephone)} varchar(12),
                                                    {nameof(PrimaryUserData.Password)} varchar(100), 

                                                    {nameof(SecondaryUserData.FirstName)} varchar(100),                                                                                                  
                                                    {nameof(SecondaryUserData.SurName)} varchar(100),                                                                                                  
                                                    {nameof(SecondaryUserData.Country)} varchar(55),                                                                                                  
                                                    {nameof(SecondaryUserData.City)} varchar(55),                                                                                                  
                                                    {nameof(SecondaryUserData.Gender)} varchar(100),                                                                                                  
                                                    {nameof(SecondaryUserData.DateOfBirthday)} DATETIME(6))                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                                                    BEGIN                                                         
                                                        INSERT INTO {nameof(PrimaryUserData)}
                                                        ({nameof(PrimaryUserData.Id)}, 
                                                            {nameof(PrimaryUserData.EmailAddress)},
                                                            {nameof(PrimaryUserData.Telephone)},
                                                            {nameof(PrimaryUserData.Password)}) 
                                                        VALUES 
                                                        ({nameof(PrimaryUserData.Id)}, 
                                                            {nameof(PrimaryUserData.EmailAddress)},
                                                            {nameof(PrimaryUserData.Telephone)},
                                                            {nameof(PrimaryUserData.Password)}) ;

                                                        INSERT INTO {nameof(SecondaryUserData)}
                                                        ({nameof(SecondaryUserData.FirstName)}, 
                                                            {nameof(SecondaryUserData.SurName)},
                                                            {nameof(SecondaryUserData.Country)},
                                                            {nameof(SecondaryUserData.City)},
                                                            {nameof(SecondaryUserData.Gender)},                                                                                                                                                                     
                                                            {nameof(SecondaryUserData.DateOfBirthday)},
                                                            {nameof(SecondaryUserData.PrimaryDataId)})                                                                                                                      
                                                        VALUES 
                                                        ({nameof(SecondaryUserData.FirstName)}, 
                                                            {nameof(SecondaryUserData.SurName)},
                                                            {nameof(SecondaryUserData.Country)},
                                                            {nameof(SecondaryUserData.City)},
                                                            {nameof(SecondaryUserData.Gender)},                                                                                                                                                                       
                                                            {nameof(SecondaryUserData.DateOfBirthday)},
                                                            {nameof(PrimaryUserData.Id)});                                                        
                                                        COMMIT;
                                                    END;";
        private string UpdateUserAccount => $@"CREATE PROCEDURE {nameof(Procedures.UpdateUserAccount)}
                                                   ({nameof(PrimaryUserData.Id)} VARCHAR(36),
                                                    {nameof(PrimaryUserData.EmailAddress)} varchar(255),
                                                    {nameof(PrimaryUserData.Telephone)} varchar(12),
                                                    {nameof(PrimaryUserData.Password)} varchar(100), 

                                                    {nameof(SecondaryUserData.FirstName)} varchar(100),                                                                                                  
                                                    {nameof(SecondaryUserData.SurName)} varchar(100),                                                                                                  
                                                    {nameof(SecondaryUserData.Country)} varchar(55),                                                                                                  
                                                    {nameof(SecondaryUserData.City)} varchar(55),                                                                                                  
                                                    {nameof(SecondaryUserData.Gender)} varchar(100),                                                                                                  
                                                    {nameof(SecondaryUserData.DateOfBirthday)} DATETIME(6))                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                                                    BEGIN   

                                                        IF {nameof(PrimaryUserData.EmailAddress)} IS NOT NULL AND {nameof(PrimaryUserData.Telephone)} IS NOT NULL THEN    
                                                             UPDATE {nameof(PrimaryUserData)} as _ut SET _ut.{nameof(PrimaryUserData.EmailAddress)} = {nameof(PrimaryUserData.EmailAddress)}
                                                                    , _ut.{nameof(PrimaryUserData.Telephone)} = {nameof(PrimaryUserData.Telephone)} 
                                                                    WHERE _ut.{nameof(PrimaryUserData.Id)} = {nameof(PrimaryUserData.Id)};
                                                        ELSEIF {nameof(PrimaryUserData.EmailAddress)} IS NOT NULL THEN
                                                             UPDATE {nameof(PrimaryUserData)} as _ut SET _ut.{nameof(PrimaryUserData.EmailAddress)} = {nameof(PrimaryUserData.EmailAddress)}
                                                                    WHERE _ut.{nameof(PrimaryUserData.Id)} = {nameof(PrimaryUserData.Id)};
                                                        ELSEIF {nameof(PrimaryUserData.Telephone)} IS NOT NULL THEN
                                                             UPDATE {nameof(PrimaryUserData)} as _ut SET _ut.{nameof(PrimaryUserData.Telephone)} = {nameof(PrimaryUserData.Telephone)} 
                                                                    WHERE _ut.{nameof(PrimaryUserData.Id)} = {nameof(PrimaryUserData.Id)};                                                          
                                                        END IF;  

                                                        UPDATE {nameof(PrimaryUserData)} AS _pm
                                                        SET                                                                                                                        
                                                             _pm.{nameof(PrimaryUserData.Password)} = {nameof(PrimaryUserData.Password)} 
                                                              WHERE _pm.{nameof(PrimaryUserData.Id)} = {nameof(PrimaryUserData.Id)};  

                                                        UPDATE {nameof(SecondaryUserData)} AS _pm
                                                        SET                                                         
                                                             _pm.{nameof(SecondaryUserData.FirstName)} = {nameof(SecondaryUserData.FirstName)},
                                                             _pm.{nameof(SecondaryUserData.SurName)} = {nameof(SecondaryUserData.SurName)},
                                                             _pm.{nameof(SecondaryUserData.Country)} = {nameof(SecondaryUserData.Country)},
                                                             _pm.{nameof(SecondaryUserData.City)} = {nameof(SecondaryUserData.City)},
                                                             _pm.{nameof(SecondaryUserData.Gender)} = {nameof(SecondaryUserData.Gender)},
                                                             _pm.{nameof(SecondaryUserData.DateOfBirthday)} = {nameof(SecondaryUserData.DateOfBirthday)}                                                                                                                      
                                                              WHERE  _pm.{nameof(SecondaryUserData.PrimaryDataId)} = {nameof(PrimaryUserData.Id)}; 

                                                        COMMIT;
                                                    END;";
        private string GetAccount => $@"CREATE PROCEDURE {nameof(Procedures.GetAccount)}
                                                   ({nameof(PrimaryUserData.Id)} VARCHAR(36))
                                                        BEGIN
                                                            SELECT 
                                                                   _pm.{nameof(PrimaryUserData.Id)},
                                                                   _pm.{nameof(PrimaryUserData.EmailAddress)}, 
                                                                   _pm.{nameof(PrimaryUserData.Telephone)},
                                                                   _pm.{nameof(PrimaryUserData.Password)}, 
                                                                   _su.{nameof(SecondaryUserData.FirstName)}, 
                                                                   _su.{nameof(SecondaryUserData.SurName)}, 
                                                                   _su.{nameof(SecondaryUserData.DateOfBirthday)}, 
                                                                   _su.{nameof(SecondaryUserData.Gender)}, 
                                                                   _su.{nameof(SecondaryUserData.Country)}, 
                                                                   _su.{nameof(SecondaryUserData.City)}                                                                  
                                                                    FROM (SELECT * FROM {nameof(PrimaryUserData)} AS _pu  WHERE _pu.{nameof(PrimaryUserData.Id)} = {nameof(PrimaryUserData.Id)}) AS _pm                                                                                                                                                                        
                                                                JOIN {nameof(SecondaryUserData)} AS _su 
                                                                ON  _pm.{nameof(PrimaryUserData.Id)} = _su.{nameof(SecondaryUserData.PrimaryDataId)};                                                                
                                                        END;";
        private string Login => $@"CREATE PROCEDURE {nameof(Procedures.Login)}
                                                   ({nameof(PrimaryUserData.EmailAddress)} VARCHAR(255),{nameof(PrimaryUserData.Password)} VARCHAR(255))
                                                        BEGIN
                                                            SELECT 
                                                                   _pm.{nameof(PrimaryUserData.Id)},
                                                                   _pm.{nameof(PrimaryUserData.EmailAddress)}, 
                                                                   _pm.{nameof(PrimaryUserData.Telephone)},
                                                                   _pm.{nameof(PrimaryUserData.Password)}, 
                                                                   _su.{nameof(SecondaryUserData.FirstName)}, 
                                                                   _su.{nameof(SecondaryUserData.SurName)}, 
                                                                   _su.{nameof(SecondaryUserData.DateOfBirthday)}, 
                                                                   _su.{nameof(SecondaryUserData.Gender)}, 
                                                                   _su.{nameof(SecondaryUserData.Country)}, 
                                                                   _su.{nameof(SecondaryUserData.City)}                                                                  
                                                                    FROM (SELECT * FROM {nameof(PrimaryUserData)} AS _pu 
                                                                    WHERE _pu.{nameof(PrimaryUserData.EmailAddress)} = {nameof(PrimaryUserData.EmailAddress)}
                                                                    AND _pu.{nameof(PrimaryUserData.Password)} = {nameof(PrimaryUserData.Password)}) AS _pm                                                                                                                                                                        
                                                                JOIN {nameof(SecondaryUserData)} AS _su 
                                                                ON  _pm.{nameof(PrimaryUserData.Id)} = _su.{nameof(SecondaryUserData.PrimaryDataId)};                                                                
                                                        END;";
        private string DeleteAccount => $@"CREATE PROCEDURE {nameof(Procedures.DeleteAccount)}
                                                   ({nameof(PrimaryUserData.Id)} VARCHAR(36))
                                                        BEGIN
                                                            DELETE FROM {nameof(PrimaryUserData)} 
                                                                WHERE  {nameof(PrimaryUserData.Id)} = {nameof(PrimaryUserData.Id)};
                                                        END;" ;
        private string CheckingEmailOnUnique => $@"CREATE PROCEDURE {nameof(Procedures.CheckingMailOnUnique)}
                                                   ({nameof(PrimaryUserData.EmailAddress)} varchar(255))
                                                        BEGIN
                                                            SELECT EXISTS (SELECT * FROM {nameof(PrimaryUserData)} AS _pm WHERE _pm.{nameof(PrimaryUserData.EmailAddress)} = {nameof(PrimaryUserData.EmailAddress)}) AS {nameof(Alias.EmailUnuque)};  
                                                        END;";
        private string CheckingNumberOnUnique => $@"CREATE PROCEDURE {nameof(Procedures.CheckingNumberOnUnique)}
                                                   ({nameof(PrimaryUserData.Telephone)} VARCHAR(12))
                                                        BEGIN
                                                            SELECT EXISTS (SELECT * FROM {nameof(PrimaryUserData)} AS _pm WHERE _pm.{nameof(PrimaryUserData.Telephone)} = {nameof(PrimaryUserData.Telephone)}) AS {nameof(Alias.NumberUnique)};  
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

                    MySqlCommand command = new MySqlCommand("CREATE DATABASE PET_ACCOUNT_DB", connection2);
                         command.ExecuteNonQuery();
                }
                using (MySqlConnection connection2 = new MySqlConnection(connectionString))
                {
                    connection2.Open();

                    MySqlCommand command = new MySqlCommand(AddTableToDB, connection2);
                    command.ExecuteNonQuery();
                    command.CommandText = CreateAccount;
                    command.ExecuteNonQuery();
                    command.CommandText = UpdateUserAccount;
                    command.ExecuteNonQuery();
                    command.CommandText = DeleteAccount;
                    command.ExecuteNonQuery();
                    command.CommandText = CheckingEmailOnUnique;
                    command.ExecuteNonQuery();
                    command.CommandText = CheckingNumberOnUnique;
                    command.ExecuteNonQuery();
                    command.CommandText = GetAccount;
                    command.ExecuteNonQuery();
                    command.CommandText = Login;
                    command.ExecuteNonQuery();

                    //default user (Admin)
                    byte count = 1;
                    command.CommandText = $@"CALL {nameof(Procedures.CreateAccount)} (
                        '{new Guid($"00000000-0000-0000-0000-00000000000" + count)}',
                        '11{count}@111.com', '{new string("1234" + count)}',
                        'Admin', 'Admin', 'Admin', 'Admin','Admin','Admin',
                        ADDDATE('2000-01-01', INTERVAL 0 DAY))";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
