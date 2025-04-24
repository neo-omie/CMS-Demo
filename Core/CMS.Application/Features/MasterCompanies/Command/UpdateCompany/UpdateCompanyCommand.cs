using CMS.Application.Features.Document;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.UpdateCompany
{
    public record UpdateCompanyCommand(int id, MasterCompany comp):IRequest<MasterCompany>;
    
}
