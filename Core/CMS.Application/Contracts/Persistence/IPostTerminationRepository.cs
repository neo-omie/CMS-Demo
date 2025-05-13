using CMS.Application.Features.MasterCompanies;
using CMS.Application.Features.PostTermination.Command.AddCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Contracts.Persistence
{
   public  interface IPostTerminationRepository
    {
        Task<bool> AddTerminationDetailsAsync(int contractId, TerminationDocumentUploadDto _terminationDocumentUploadDto);

    }
}
