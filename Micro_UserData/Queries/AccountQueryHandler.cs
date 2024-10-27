using MediatR;
using Micro_Account.Models;
using Micro_Person.Services.Repository;
using System.Data;

namespace Micro_Person.Queries
{
    public class AccountQuery : IRequest<UserDTO> 
    {
        public readonly string Id;
        public readonly string Login;
        public readonly string Password;
        public AccountQuery(in string id) => Id = id;
        public AccountQuery(in string login, in string password) => (Login, Password) = (login, password);
    }

    public class AccountQueryHandler : IRequestHandler<AccountQuery, UserDTO>
    {
        private readonly IRepository_Me_User repository;
        public AccountQueryHandler(IRepository_Me_User repository) => this.repository = repository;
        public async Task<UserDTO> Handle(AccountQuery request, CancellationToken cancellationToken)
        {           
            if (request.Id is not null)
            {
                DataSet response = await repository.GetAccount(request.Id);
                return response.Tables[0].Rows.Count > 0 ? new UserDTO(response.Tables[0].Rows[0]) : default;
            }
            else
            {
                DataSet response = await repository.Login(request.Login, request.Password);               
                return response.Tables[0].Rows.Count  > 0  ? new UserDTO(response.Tables[0].Rows[0]) : default;
            }
        }
    }
}
