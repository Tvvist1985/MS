
using Micro_Person.Services.SQL;
using System.Data;

namespace Micro_Account.Models
{
    public class UserDTO 
    {
        public UserDTO() { }

        public UserDTO(DataRow dataRow) 
        {            
            Id = new Guid(dataRow[nameof(PrimaryUserData.Id)].ToString());
            EmailAdress = dataRow[nameof(PrimaryUserData.EmailAddress)].ToString();
            Telephone = Convert.ToInt32(dataRow[nameof(PrimaryUserData.Telephone)]);
            Password = dataRow[nameof(PrimaryUserData.Password)].ToString();
            
            FirstName = dataRow[nameof(SecondaryUserData.FirstName)].ToString();
            SurName = dataRow[nameof(SecondaryUserData.SurName)].ToString();
            Gender = dataRow[nameof(SecondaryUserData.Gender)].ToString();
            Country = dataRow[nameof(SecondaryUserData.Country)].ToString();
            City = dataRow[nameof(SecondaryUserData.City)].ToString();
            DateOfBirthday = Convert.ToDateTime(dataRow[nameof(SecondaryUserData.DateOfBirthday)]);
        }


        public Guid? Id { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public DateTime DateOfBirthday { get; set; }

        public string Gender { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string? EmailAdress { get; set; }

        public int? Telephone { get; set; }

        public string Password { get; set; }

        public string? JWT { get; set; }
    }

}
