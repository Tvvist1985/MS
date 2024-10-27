using MediatR;
using Micro_Account.Models;
using Micro_Account.Services.CheckingOnUnique;
using Micro_Person.Services.Repository;

namespace Micro_Account.Queries
{
    public class CheckingOnUniqueQuery : IRequest<ResponseMessages>
    {
        public readonly string Email;
        public readonly string Number;

        public CheckingOnUniqueQuery(in string email, in string number) => (Email, Number) = (email, number);        
    }

    public class CheckingOnUniqueQueryHandler : IRequestHandler<CheckingOnUniqueQuery, ResponseMessages>
    {
        private readonly IRepository_Me_User repository;

        public CheckingOnUniqueQueryHandler(IRepository_Me_User repository) => this.repository = repository;

        public async Task<ResponseMessages> Handle(CheckingOnUniqueQuery request, CancellationToken cancellationToken)
        {
            var res = await repository.CheckingOnUnique(request.Email, request.Number);

            new UniquenessData().CheckingOnUnique(res, out ResponseMessages errors);
            return errors;
        }       
    }
}
