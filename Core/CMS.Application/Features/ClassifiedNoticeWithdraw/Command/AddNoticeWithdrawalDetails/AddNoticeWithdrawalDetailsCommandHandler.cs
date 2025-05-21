using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ClassifiedNoticeWithdraw.Command.AddNoticeWithdrawalDetails
{
    public class AddNoticeWithdrawalDetailsCommandHandler : IRequestHandler<AddNoticeWithdrawalDetailsCommand, bool>
    {
        
        IClassifiedContractRepository _classifiedContractRepository;
        public AddNoticeWithdrawalDetailsCommandHandler(IClassifiedContractRepository classifiedContractRepository)
        {
            _classifiedContractRepository = classifiedContractRepository;
        }
        public async Task<bool> Handle(AddNoticeWithdrawalDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _classifiedContractRepository.AddNoticeWithdrawalDetailsAsync(request.contractId, request.postTermId, request.noticeWithdrawalDocumentUploadDto);
        }
    }
}
