using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.DeleteCompany
{
    public record DeleteCompanyCommand(int id) :IRequest<bool>;
    
}
