using Mediator_Email.Services.SMTP;
using MediatR;

namespace Mediator_Email.Commands
{
    public class SMPTCommand : IRequest<string> 
    {
        public readonly string Email;
        public SMPTCommand(in string email) => Email = email;
    }

    public class SMPTCommandHandler : IRequestHandler<SMPTCommand, string>
    {
        private readonly ISMTP sMTP;
        public SMPTCommandHandler(ISMTP sMTP) => this.sMTP = sMTP;
                            
        public async Task<string> Handle(SMPTCommand request, CancellationToken cancellationToken)
        {            
            return sMTP.SendPinCodeToEmail(request.Email);
        }                                    
    }
}
