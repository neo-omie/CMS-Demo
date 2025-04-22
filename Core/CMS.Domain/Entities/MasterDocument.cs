
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Domain.Constants;

namespace CMS.Domain.Entities
{
    public class MasterDocument
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValueId { get; set; }

        [Required]
        public string DocumentName { get; set; }
        public Status status { get; set; } 

        public bool IsDeleted { get; set; } =false ;
       
    }

    
}
