using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.PostTermination.Command.AddCommand
{
    public class TerminationDocumentUploadDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public int Notice_Duration { get; set; } = 0;

        [Required]
        public DateTime End_Date { get; set; }

        [Required]
        public string Remark { get; set; }

    }
}
