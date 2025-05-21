using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CMS.Application.Features.ClassifiedNoticeWithdraw.Command.AddNoticeWithdrawalDetails
{
    public class NoticeWithdrawalDocumentUploadDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}
