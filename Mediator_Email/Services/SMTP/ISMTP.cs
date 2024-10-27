namespace Mediator_Email.Services.SMTP
{
    public interface ISMTP
    {
        public string SendPinCodeToEmail(in string email);
        public void RecoveryPassword(in string email, in string message);
    }
}
