using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.AuditTrails.Queries.GetAllAudits;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly CMSDbContext _context;

        public AuditTrailRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllAuditDto>> GetAllAuditTrail(int pageNumber, int pageSize)
        {
            string query = "EXEC SP_GetAllAudits @pageNumber={0} , @pageSize={1}";
            var doc = await _context.GetAllAudits.FromSqlRaw(query, pageNumber, pageSize).ToListAsync();
            doc.ForEach(x => { x.TableName = ((TableList)x.ForTable).ToString();
                x.StatusName = ((LogStatus)x.Status).ToString();
            });
            return doc;

        }
    }
}
