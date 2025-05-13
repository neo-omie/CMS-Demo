using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Commands.ContractApproveApprover;
using CMS.Application.Features.Contracts.Commands.CreateNewContract;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.ContractApprove
{
    public class ContractApproveCommandHandler : IRequestHandler<ContractApproveCommand, Contract>
    {
            readonly IContractRepository _contractRepository;
            public ContractApproveCommandHandler(IContractRepository contractRepository)
            {
                _contractRepository = contractRepository;
            }
            public async Task<Contract> Handle(ContractApproveCommand request, CancellationToken cancellationToken)
            {
                return await _contractRepository.ApproveContract(request.id, request.empCode);
            }
        
    }
}
