using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class AddendumContractRepository : IAddendumContractRepository
    {
        private readonly CMSDbContext _dbContext;

        public AddendumContractRepository(CMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AddendumContract> AddAddendumContractAsync(int id, AddendumContract addendumContract)
        {
            var contractIdCheck = await _dbContext.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (contractIdCheck == null) {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }

            //contractIdCheck.ContractName = cp.ContractName;
            contractIdCheck.DepartmentId = addendumContract.DepartmentId;
            contractIdCheck.ContractWithCompanyId = addendumContract.ContractWithCompanyId;
            contractIdCheck.ContractTypeId = addendumContract.ContractTypeId;
            contractIdCheck.ApostilleTypeId = addendumContract.ApostilleTypeId;
            contractIdCheck.ActualDocRefNo = addendumContract.ActualDocRefNo;
            contractIdCheck.RetainerContract = addendumContract.RetainerContract;
            contractIdCheck.TermsAndConditions = addendumContract.TermsAndConditions;
            contractIdCheck.ValidFrom = addendumContract.ValidFrom;
            contractIdCheck.ValidTill = addendumContract.ValidTill;
            contractIdCheck.EmpCustodianId = addendumContract.EmpCustodianId;
            contractIdCheck.IsDeleted = addendumContract.IsDeleted;

            ////Employee Details 
            //contractIdCheck.EmployeeName = addendumContract.EmployeeName;
            //contractIdCheck.EmployeeCode = addendumContract.EmployeeCode;
            //contractIdCheck.EmployeeMobile = addendumContract.EmployeeMobile;
            //contractIdCheck.Email = addendumContract.Email;
            //contractIdCheck.Role = addendumContract.Role;
            //contractIdCheck.EmpDepartmentId = addendumContract.EmpDepartmentId;
            //contractIdCheck.Unit = addendumContract.Unit;


            ////Company Details 
            //contractIdCheck.CompanyName = addendumContract.CompanyName;
            //contractIdCheck.CompanyContactNo = addendumContract.CompanyContactNo;
            //contractIdCheck.CompanyEmailId = addendumContract.CompanyEmailId;
            //contractIdCheck.CompanyAddressLine1 = addendumContract.CompanyAddressLine1;
            //contractIdCheck.CompanyAddressLine2 = addendumContract.CompanyAddressLine2;
            //contractIdCheck.CompanyAddressLine3 = addendumContract.CompanyAddressLine3;
            //contractIdCheck.Zipcode = addendumContract.Zipcode;
            //contractIdCheck.CountryId = addendumContract.CountryId;
            //contractIdCheck.StateId = addendumContract.StateId;
            //contractIdCheck.CityId = addendumContract.CityId;
            //contractIdCheck.PocName = addendumContract.PocName;
            //contractIdCheck.PocContactNumber = addendumContract.PocContactNumber;
            //contractIdCheck.PocEmailId = addendumContract.PocEmailId;

            _dbContext.ContractsEntity.Update(contractIdCheck);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return addendumContract;
            }
            throw new Exception("Failed to add Addendum Contract");
        }
    }
}
