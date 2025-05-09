
using CMS.Domain.Constants;

namespace CMS.Application.Features.AddendumContract.AddendumContractDto
{
    public class AddAddendumContractDto
    {
        //Contracts Details 
        public int ContractId { get; set; }
        public int DepartmentId { get; set; }
        public int ContractWithCompanyId { get; set; }
        public int ContractTypeId { get; set; }
        public int ApostilleTypeId { get; set; }
        public int ActualDocRefNo { get; set; }
        public RetainerType RetainerContract { get; set; }

        public string TermsAndConditions { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public int EmpCustodianId { get; set; }

        //public bool IsDeleted { get; set; } = false;

        //MasterEmployees Details
        //public string EmployeeName { get; set; }
        //public string EmployeeCode { get; set; }
        //public long EmployeeMobile { get; set; }
        //public string Email { get; set; }
        //public string Role { get; set; }
        //public int EmpDepartmentId { get; set; }
        //public string Unit {  get; set; }

        //MasterCompany Details 
        //public string CompanyName { get; set; }
        //public long CompanyContactNo { get; set; }
        //public string CompanyEmailId { get; set; }
        //public string CompanyAddressLine1 { get; set; }
        //public string CompanyAddressLine2 { get; set; }
        //public string CompanyAddressLine3 { get; set; }
        //public int Zipcode { get; set; }
        //public int CountryId { get; set; }
        //public int StateId { get; set; }
        //public int CityId { get; set; }
        //public string PocName { get; set; }
        //public long PocContactNumber { get; set; }
        //public string PocEmailId { get; set; }

    }
}
