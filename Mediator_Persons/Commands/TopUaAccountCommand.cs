using Mediator_BankAccount.Services.IRepository;
using MediatR;

namespace Mediator_BankAccount.Commands
{
    public class TopUaAccountCommand : IRequest
    {
        public readonly int Sum;
        public readonly string Id;
        public TopUaAccountCommand(in string id , in int sum)=> (Id, Sum) = (id, sum);
    }

    public class TopUaAccountCommandHandler : IRequestHandler<TopUaAccountCommand>
    {

        private readonly IRepository_Bank_Account repository;

        public TopUaAccountCommandHandler(IRepository_Bank_Account repository) => this.repository = repository;

        public async Task Handle(TopUaAccountCommand request, CancellationToken cancellationToken)
        {
            await repository.ToUpAccount(request.Id, request.Sum);
        }
    }
}
