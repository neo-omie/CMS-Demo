using CMS.Application.Features.MasterCompanies;
using CMS.Application.Features.PostTermination.Command.AddCommand;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
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
        public Task<Contract> ApproveTerminateContract(int id, string empCode, ContractStatus status,string subject,string emailBody);
    }
}
