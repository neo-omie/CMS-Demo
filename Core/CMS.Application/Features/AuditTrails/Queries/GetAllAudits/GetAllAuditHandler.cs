using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.AuditTrails.Queries.GetAllAudits
{
    public class GetAllAuditHandler : IRequestHandler<GetAllAuditsQuery, IEnumerable<GetAllAuditDto>>
    {
        private readonly IAuditTrailRepository _auditTrailRepository;
       
        public GetAllAuditHandler(IAuditTrailRepository auditTrailRepository) { 
        
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<IEnumerable<GetAllAuditDto>> Handle(GetAllAuditsQuery request, CancellationToken cancellationToken)
        {
            return await _auditTrailRepository.GetAllAuditTrail(request.pageNumber, request.pageSize);
        }
    }
}
