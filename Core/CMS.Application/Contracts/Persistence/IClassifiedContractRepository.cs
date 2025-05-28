
using CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById;
using CMS.Application.Features.ClassifiedNoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using CMS.Application.Features.ClassifiedPostTermination.Command.AddCommand;
using CMS.Domain.Constants;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IClassifiedContractRepository
    {
        Task<ContractsCount> GetClassifiedContractsCountAsync();
        Task<IEnumerable<GetAllClassifiedContractsDto>> GetAllClassifiedContractsAsync(int pageNumber, int pageSize);
        Task<GetClassifiedContractByIdDto> GetClassifiedContractByIdAsync(int id);

         Task<ClassifiedContract> ApproveRejectContract(int id, string empCode, ContractStatus status);

        Task<bool> AddNoticeWithdrawalDetailsAsync(int contractId, int postTermId, NoticeWithdrawalDocumentUploadDto noticeWithdrawalDocumentUploadDto);

        Task<bool> AddTerminationDetailsAsync(int contractId, TerminationDocumentUploadDto _terminationDocumentUploadDto);
        Task<ClassifiedContract> ApproveTerminateContract(int id, string empCode, ContractStatus status, string subject, string emailBody);

        Task<ClassifiedContract> ApproveNoticeWithdrawalAsync(int id, string empCode, ContractStatus status, string subject, string emailBody);

        
        Task<ClassifiedContract> AddClassifiedContractAsync(ClassifiedContract cp,string empName);
        Task<bool> UpdateClassifiedContractAsync(int id, ClassifiedContract cp);
        Task<bool> DeleteClassifiedContractAsync(int id, string empCode);
    }
}
