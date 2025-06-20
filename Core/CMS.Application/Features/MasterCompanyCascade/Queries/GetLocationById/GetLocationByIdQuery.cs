using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;

namespace CMS.Application.Features.MasterCompanyCascade.Queries.GetLocationById
{
    public record GetLocationByIdQuery(int cityId):IRequest<ListOfLocation>;
    
}
