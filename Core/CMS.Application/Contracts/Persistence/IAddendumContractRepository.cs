using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IAddendumContractRepository
    {
        Task<AddendumContract> AddAddendumContractAsync(int id, AddendumContract addendumContract);
    }
}
