namespace Micro_Person.Services.SQL
{
    public enum PrimaryUserData : byte
    {
        Id,
        EmailAddress,
        Telephone,
        Password
    }

    public enum SecondaryUserData : byte
    {
        Id,
        FirstName,
        SurName,
        DateOfBirthday,
        Gender,
        Country,
        City,
        PrimaryDataId
    }

    public enum Procedures : byte
    {
        CreateAccount,
        UpdateUserAccount,
        GetAccount,
        Login,
        DeleteAccount,
        CheckingMailOnUnique,
        CheckingNumberOnUnique

    }

    public enum Alias: byte { EmailUnuque = 0, NumberUnique = 1}


}
