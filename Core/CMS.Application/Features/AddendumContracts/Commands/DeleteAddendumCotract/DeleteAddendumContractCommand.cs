using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.AddendumContracts.Commands.DeleteAddendumCotract
{
    public record DeleteAddendumContractCommand(int AddendumId):IRequest<bool>;
}
