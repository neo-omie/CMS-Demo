using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.AddCompany
{
    public record AddCompanyCommand(MasterCompany mastercomp):IRequest<MasterCompany>;
    
}
