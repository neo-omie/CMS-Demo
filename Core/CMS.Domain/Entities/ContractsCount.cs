using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class ContractsCount
    {
        public int AllContractsCount { get; set; }
        public int PendingApprovalContractsCount { get; set; }
        public int PendingTerminationContractsCount { get; set; }
        public int ExpiredContractsCount { get; set; }
        public int ActiveContractsCount { get; set; }
        public int TerminatedContractsCount { get; set; }
    }
}
