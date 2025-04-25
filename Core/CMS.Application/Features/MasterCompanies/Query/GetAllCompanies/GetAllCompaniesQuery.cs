using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Query.GetAllCompanies
{
    public record GetAllCompaniesQuery( string searchTerm, int pageNumber, int pageSize) : IRequest<IEnumerable<GetMastersDTO>>;
}
