using Micro_Account.Models;
using Micro_Person.Services.SQL;
using System.Data;

namespace Micro_Account.Services.CheckingOnUnique
{
    public class UniquenessData
    {
        public bool CheckingOnUnique(DataSet dataSet, out ResponseMessages errors)
        {
            errors = new ResponseMessages();

            if (Convert.ToBoolean(dataSet.Tables[(byte)Alias.EmailUnuque].Rows[0][nameof(Alias.EmailUnuque)]))
                errors.EmailError = true;

            if (Convert.ToBoolean(dataSet.Tables[(byte)Alias.NumberUnique].Rows[0][nameof(Alias.NumberUnique)]))
                errors.NumberError = true;

            return errors.NumberError || errors.EmailError;
        }
    }
}
