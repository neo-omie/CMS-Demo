using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        readonly CMSDbContext _context;
        public ContractRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetAllContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.CountAsync();
            return _context.ContractsEntity.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Where(ce => ce.IsDeleted == false)
                .Select(a => new GetAllContractsDto
                {
                    ContractID = a.ContractId,
                    ContractName = a.ContractName,
                    ContractType = a.ContractType.ContractTypeName,
                    DepartmentName = a.Department.Department.DepartmentName,
                    EffectiveDate = a.ValidFrom,
                    ExpiryDate = a.ValidTill,
                    ToBeRenewedOn = a.RenewalFrom,
                    AddendumDate = DateTime.Now.AddMonths(3),
                    Status = a.Approver3Status,
                    ApprovalPendingFrom = a.Department.Approver3.EmployeeName,
                    RenewalContractPerson = a.Department.Approver3.EmployeeName,
                    RenewalDueIn = (a.RenewalTill - DateTime.Now).ToString(),
                    Location = a.Location,
                    TotalRecords = totalRecords
                });
        }

        public async Task<GetContractByIdDto> GetContractByIdAsync(int id)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if(foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            return await _context.ContractsEntity
                .Where(ce => ce.ContractId == id)
                .Select(ce => new GetContractByIdDto
            {
                ContractId = ce.ContractId,
                ContractName = ce.ContractName,
                DepartmentId = ce.DepartmentId,
                DepartmentName = ce.Department.Department.DepartmentName,
                ContractWithCompanyId = ce.ContractWithCompanyId,
                ContractWithCompanyName = ce.ContractWithCompany.CompanyName,
                ContractTypeId = ce.ContractTypeId,
                ContractTypeName = ce.ContractType.ContractTypeName,
                ApostilleTypeId = ce.ApostilleTypeId,
                ApostilleTypeName = ce.ApostilleType.ApostilleName,
                ActualDocRefNo = ce.ActualDocRefNo,
                RetainerContract = ce.RetainerContract,
                TermsAndConditions = ce.TermsAndConditions,
                ValidFrom = ce.ValidFrom,
                ValidTill = ce.ValidTill,
                RenewalFrom = ce.RenewalFrom,
                RenewalTill = ce.RenewalTill,
                AddendumDate = ce.AddendumDate,
                EmpCustodianId = ce.EmpCustodianId,
                EmpCustodianName = ce.EmpCustodian.EmployeeName,
                Location = ce.Location,
                Approver1Status = ce.Approver1Status,
                Approver2Status = ce.Approver2Status,
                Approver3Status = ce.Approver3Status,
                IsDeleted = ce.IsDeleted
            }).FirstOrDefaultAsync();

        }
        public async Task<Contract> AddContractAsync(Contract cp)
        {
            var addedContract = await _context.ContractsEntity.AddAsync(cp);
            if(await _context.SaveChangesAsync() > 0)
                return cp;
            throw new Exception("For some reasons, contract has not been added.");
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            foundContract.IsDeleted = true;
            _context.ContractsEntity.Update(foundContract);
            if(await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }


        public async Task<bool> UpdateContractAsync(int id, Contract cp)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            cp.ContractId = foundContract.ContractId;
            _context.ContractsEntity.Update(cp);
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
