
namespace Micro_Account.Exceptions
{
    public class RequestToDBException : Exception
    {
        public RequestToDBException() : base("The database is not available.") { }                            
        public RequestToDBException(in string message) : base(message) { }
    }
}
