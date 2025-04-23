using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU
{
    public class UpdateApprovalMatrixMOUDto
    {
        [Required]
        public string ApproverId1 { get; set; }

        [Required]
        public string ApproverId2 { get; set; }

        [Required]
        public string ApproverId3 { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
    }
}
