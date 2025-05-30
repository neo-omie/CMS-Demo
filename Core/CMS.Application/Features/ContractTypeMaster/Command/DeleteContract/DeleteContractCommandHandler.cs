using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Command.DeleteContract
{
    public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, bool>
    {
        private readonly IContractTypeMasterRepository _contractTypeMasterRepository;
        private readonly ICacheService _cacheService;
        public DeleteContractCommandHandler(IContractTypeMasterRepository contractTypeMasterRepository, ICacheService cacheService)
        {
            _contractTypeMasterRepository = contractTypeMasterRepository;
            _cacheService = cacheService;
        }
        public async Task<bool> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            var result=await  _contractTypeMasterRepository.DeletContract(request.id,request.empCode);

            if (result)
            {
                string cacheKey = $"contracts_1_10";
                await _cacheService.RemoveAsync(cacheKey);
            }

            return result;
        }
    }
}
