
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace CMS.Domain.Entities
{
    public class MasterDocument
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValueId { get; set; }

        [Required]
        public string DisplayDocumentName { get; set; }
        [Required]
        public string DocumentPath { get; set; }
        [Required]
        public string UniqueDocumentName { get; set; }
        [Required]
        public Status status { get; set; }


        public bool IsDeleted { get; set; } = false;

    }


}
