using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities.CompanyMaster
{
    public class MasterCompany
    {
        [Key]
        public int ValueId { get; set; }
        public string CompanyName { get; set; }

        public string PocName { get; set; }

        
        public bool CompanyStatus { get; set; } = true;

        public int PocContactNumber { get; set; }

        public string PocEmailId { get; set; }


        //Company Contact details

        public string CompanyAddressLine1 { get; set; }
        public string CompanyAddressLine2 { get; set; }
        public string CompanyAddressLine3 { get; set; }

        public int Zipcode { get; set; }
        public int CompanyContactNo { get; set; }

        public string CompanyEmailId { get; set; }

        public string CompanyWebsiteUrl { get; set; }


        //Company Bank/Other Details

        public string CompanyBankName { get; set; }

        public int GSTno { get; set; }

        public int BankAccNo { get; set; }

        public int MSMERegistrationNo { get; set; }

        public string IFSCCode { get; set; }

        public string PanNo { get; set; }


        //for soft delete 
        public bool IsDeleted { get; set; } = false;

      


    }
}
