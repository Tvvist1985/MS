using Mediator_BankAccount.Exceptions;
using Mediator_BankAccount.Models;
using Mediator_BankAccount.Services.IRepository;
using MediatR;

namespace Mediator_BankAccount.Commands
{
    public class BankAccountCommand : IRequest<BankAccountDTO>
    {
        public readonly string Id;
        public BankAccountCommand(in string id) => Id = id;
    }

    public class BankAccountCommandHandler : IRequestHandler<BankAccountCommand, BankAccountDTO>
    {
        private readonly IRepository_Bank_Account repository;

        public BankAccountCommandHandler(IRepository_Bank_Account repository) => this.repository = repository;

        public async Task<BankAccountDTO> Handle(BankAccountCommand request, CancellationToken cancellationToken)
        {
            Random rand = new Random();

            byte counter = 10;
            while (counter > 0)
            {
                try
                {
                    var accNumb = rand.Next(1, 99999999);
                    await repository.CreateBankAccount(request.Id, accNumb);
                    return new BankAccountDTO(accNumb, 0.0);
                }
                catch (Exception ex)
                {
                    counter--;
                }
            }

            throw new CreateAccountException("Failed to create an bank account.");
        }
    }
}
