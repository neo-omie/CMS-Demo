using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public class MasterApprovalMatrixMOU
    {
        [Key]
        public int MasterApprovalMatrixMOUId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string ApproverId1 { get; set; }

        [Required]
        public string ApproverId2 { get; set; }

        [Required]
        public string ApproverId3 { get; set; }
        [Required]
        public int NumberOfDays { get; set; }

        public Department Department { get; set; }
        public MasterEmployee Approver1 { get; set; }
        public MasterEmployee Approver2 { get; set; }
        public MasterEmployee Approver3 { get; set; }
    }
}
