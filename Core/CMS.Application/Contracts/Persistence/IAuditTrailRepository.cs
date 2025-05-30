using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.AuditTrails.Queries.GetAllAudits;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public  interface IAuditTrailRepository
    {
        Task<IEnumerable<GetAllAuditDto>> GetAllAuditTrail(int pageNumber ,int pageSize);
    }
}
