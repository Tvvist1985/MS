namespace Mediator_BankAccount.Exceptions
{
    internal class CreateAccountException : Exception
    {
        public CreateAccountException(in string message) : base(message) { }                            
    }
}
