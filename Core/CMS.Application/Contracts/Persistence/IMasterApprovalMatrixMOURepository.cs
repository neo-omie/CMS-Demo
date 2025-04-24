using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterApprovalMatrixMOURepository
    {
        public Task<IEnumerable<GetAllApprovalMatrixMOUDto>> GetAllApprovalMatrixMOU(int pageNumber, int pageSize);
        public Task<GetAllApprovalMatrixMOUByIdDto> GetApprovalMatrixMOUById(int id);
        public Task<MasterApprovalMatrixMOU> UpdateApprovalMatrixMOU(int id, UpdateApprovalMatrixMOUDto mou);
    }
}
