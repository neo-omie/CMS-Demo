using System.Net.Mail;
using System.Net;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract
{
    public class GetAllApprovalMatrixContractQueryHandler : IRequestHandler<GetAllApprovalMatrixContractQuery, IEnumerable<GetAllApprovalMatrixContractDTO>>
    {
        private readonly IMasterApprovalMatrixContractRepository _masterApprovalMatrixContractRepository;
        public GetAllApprovalMatrixContractQueryHandler(IMasterApprovalMatrixContractRepository masterApprovalMatrixContractRepository, IMapper mapper)
        {
            _masterApprovalMatrixContractRepository = masterApprovalMatrixContractRepository;
        }
        public Task<IEnumerable<GetAllApprovalMatrixContractDTO>> Handle(GetAllApprovalMatrixContractQuery request, CancellationToken cancellationToken)
        {
            return _masterApprovalMatrixContractRepository.GetAllApprovalMatrixContract(request.pageNumber,request.pageSize);
        }
    }
}
