using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PET.Models
{
    public record class UserDTO
    {
        public UserDTO() { }
        public UserDTO(UserDTO user) 
        { 
            Id = user.Id;
            FirstName = user.FirstName;
            SurName = user.SurName;
            DateOfBirth = user.DateOfBirth;
            Gender = user.Gender;
            Country = user.Country;
            City = user.City;
            EmailAdress = user.EmailAdress;
            Telephone = user.Telephone;
            Password = user.Password;
        }

        public Guid Id { get; set; } 

        [Required]
        [MaxLength(12)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(12)]
        public string SurName { get; set; } 
      
        [Required]
        //[AgeValidation(18, 50)]
        //[Range(typeof(DateTime),"18", "70")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(5)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Please, enter Valid Email (format: example@examp.com")]
        public string EmailAdress { get; set; }

        [Required]
        //[Phone]        
        public int? Telephone { get; set; }

        [Required]
        [MaxLength(50), MinLength(5)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50), MinLength(5)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
       
        public string? JWT { get; set; }
    }
}
