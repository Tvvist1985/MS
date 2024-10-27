namespace Mediator_BankAccount.Services.SQL
{
   public enum BankAccount : byte
    {
        Id,
        AccountNumber,
        Balance
    }

    public enum ChangingAccount: byte
    {
        Id,
        TimeChanging,
        Value,
        BankAccountId
    }

    public enum Procedures : byte
    {
        CreateBankAccount,
        GetBankAccount,        
        UpdateBalanse

    }
    public enum Triggers : byte
    {
       TrackingBalanse,
       ToUpAccount,

    }
}
