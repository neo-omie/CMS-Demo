using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Query.GetCompanyById
{
  public  record GetCompanyByIdQuery(int id):IRequest<MasterCompany>;

}
