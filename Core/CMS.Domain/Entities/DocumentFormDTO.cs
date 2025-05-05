using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CMS.Domain.Entities
{
    public class DocumentFormDTO
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
