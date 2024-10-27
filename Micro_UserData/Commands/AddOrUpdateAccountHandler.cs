using MediatR;
using Micro_Account.Exceptions;
using Micro_Account.Models;
using Micro_Account.Services.CheckingOnUnique;
using Micro_Person.Services.Repository;
using Micro_Person.Services.SQL;
using System.Data;

namespace Micro_Account.Commands
{
    public class AddOrUpdateCommand : UserDTO , IRequest<ResponseMessages> { }
           
    public class AddOrUpdateAccountHandler : IRequestHandler<AddOrUpdateCommand, ResponseMessages>
    {
        private readonly IRepository_Me_User repository;
      
        public AddOrUpdateAccountHandler(IRepository_Me_User repository) => this.repository = repository;

        public async Task<ResponseMessages> Handle(AddOrUpdateCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
                return await CreateAccount(request);
            else
                return await UpdateAccount(request);
        }

        private async Task<ResponseMessages> CreateAccount(AddOrUpdateCommand request)
        {
            byte count = 10;
            while (count > 0)
            {
                try
                {                    
                    request.Id = Guid.NewGuid();
                    await repository.CreateAccount(request);
                    return new ResponseMessages(request.Id.ToString());
                }
                catch
                {                   
                    var res = await repository.CheckingOnUnique(request.EmailAdress, request.Telephone.ToString());
                    if (new UniquenessData().CheckingOnUnique(res, out ResponseMessages errors))
                        return errors;
                }
                count--;
            }

            throw new RequestToDBException();
        }
    
        private async Task<ResponseMessages> UpdateAccount(AddOrUpdateCommand request)
        {
            byte count = 10;
            while (count > 0)
            {
                try
                {
                    await repository.UpdateAccount(request);
                    return new ResponseMessages() { Success = true};
                }
                catch
                {
                    var res = await repository.CheckingOnUnique(request.EmailAdress, request.Telephone.ToString());
                    if (new UniquenessData().CheckingOnUnique(res, out ResponseMessages errors))
                        return errors;                   
                }
                count--;
            }
            throw new RequestToDBException();
        }       
    }
}
