
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace CMS.Domain.Entities
{
    public class MasterDocument
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValueId { get; set; }

        [Required]
        public string DocumentName { get; set; }
        [Required]
        public string DocumentType { get; set; }
        [Required]
        public byte[] DocumentData { get; set; }
        [Required]
        public Status status { get; set; } 
        public bool IsDeleted { get; set; } = false ;
       
    }

    
}
