using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.PostTermination.Command.AddCommand
{
    public record AddPostTerminationCommand(int contractId, TerminationDocumentUploadDto modelDto) : IRequest<bool>;
    
}
