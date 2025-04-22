using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterApprovalMatrixMOURepository
    {
        public Task<IEnumerable<GetAllApprovalMatrixMOUDto>> GetAllApprovalMatrixMOU(int pageNumber, int pageSize);
        public Task<GetAllApprovalMatrixMOUByIdDto> GetApprovalMatrixMOUById(int id);
    }
}
