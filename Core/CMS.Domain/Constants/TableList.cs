using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Constants
{
    public enum TableList
    {
        Contract = 1, ClassifiedContract, AddendumContract,
        MasterContractType, MasterDepartment, MasterApostille, MasterDocument, MasterEmployee,
        MasterApprovalMatrixContract, MasterApprovalMatrixMOU, MasterEscalationMatrixContract, MasterEscalationMatrixMOU,
        PostTerminationNotice, NoticeWithdrawal
    }
}
