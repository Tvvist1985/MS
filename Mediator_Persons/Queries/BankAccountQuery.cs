using Mediator_BankAccount.Models;
using Mediator_BankAccount.Services.IRepository;
using MediatR;

namespace Mediator_BankAccount.Queries
{
    public class BankAccountQuery : IRequest<BankAccountDTO>
    {
        public readonly string Id;
        public BankAccountQuery(in string id) => Id = id;        
    }

    public class BankAccountQueryHandler : IRequestHandler<BankAccountQuery, BankAccountDTO>
    {
        private readonly IRepository_Bank_Account repository;

        public BankAccountQueryHandler(IRepository_Bank_Account repository) => this.repository = repository;
        public async Task<BankAccountDTO> Handle(BankAccountQuery request, CancellationToken cancellationToken)
        {
            var res = await repository.GetBankAccount(request.Id);
            return res.Tables[0].Rows.Count > 0 ? new BankAccountDTO(res.Tables[0].Rows[0]) : default;
        }
    }
}
