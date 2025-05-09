using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
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
        readonly IEmailService _emailService;
        readonly INotificationRepository _notificationRepository;
        public ContractRepository(CMSDbContext context, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _emailService = emailService;
            _context = context;
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetAllContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetAllContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetActiveContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetActiveContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetTerminatedContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetTerminatedContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetPendingApprovalContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetPendingApprovalContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }

        public async Task<GetContractByIdDto> GetContractByIdAsync(int id)
        {
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if(foundContract == null)
            {
                throw new NotFoundException($"Contract with ID {id} not found");
            }
            return foundContract;

        }
        public async Task<Contract> AddContractAsync(Contract cp)
        {
            var addedContract = await _context.ContractsEntity.AddAsync(cp);
            if(await _context.SaveChangesAsync() <= 0)
                throw new Exception("For some reasons, contract has not been added.");
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, cp.ContractId).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if(foundContract != null)
            {
                // To Approver L1
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          $"New Contract called '{foundContract.ContractName}' Added!",
                                          "New Contract has been added under your department. You can access and change the approvals for this contract.");
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, cp.ContractId, cp.ContractName
                );
                // To Employee Custodian
                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          $"You have added new Contract called '{foundContract.ContractName}'!",
                                          "New Contract has been added under your department.");
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, cp.ContractId, cp.ContractName
                );
            }
            return cp;
        }
        private async Task AddNewNotifications(string name, string subject, string message)
        {
            Notification createNewNotif = new Notification
            {
                EmployeeCode = name,
                NotficationSubject = subject,
                NotficationMessage = message
            };
            await _notificationRepository.NewNotification(createNewNotif);
        }
        private string GenerateEmailBody(string name, int contractID, string contractName)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>Hello {name}, new contract has been started under your department.</h1>";
            emailBody += $"<h2>Contract ID: {contractID}<br>Contract Name: {contractName}</h2>";
            emailBody += "<h3>Please check your CMS portal.</h3>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }
        public async Task SendMail(string email, string name, int contractID, string contractName)
        {
            var mailRequest = new MailRequest
            {
                Email = email,
                Subject = "New Contract Added",
                EmailBody = GenerateEmailBody(name, contractID, contractName)
            };
            await _emailService.SendEmail(mailRequest);
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
            foundContract.ContractName = cp.ContractName;
            foundContract.DepartmentId = cp.DepartmentId;
            foundContract.ContractWithCompanyId = cp.ContractWithCompanyId;
            foundContract.ContractTypeId = cp.ContractTypeId;
            foundContract.ApostilleTypeId = cp.ApostilleTypeId;
            foundContract.ActualDocRefNo = cp.ActualDocRefNo;
            foundContract.RetainerContract = cp.RetainerContract;
            foundContract.TermsAndConditions = cp.TermsAndConditions;
            foundContract.ValidFrom = cp.ValidFrom;
            foundContract.ValidTill = cp.ValidTill;
            foundContract.RenewalFrom = cp.RenewalFrom;
            foundContract.RenewalTill = cp.RenewalTill;
            foundContract.AddendumDate = cp.AddendumDate;
            foundContract.EmpCustodianId = cp.EmpCustodianId;
            foundContract.Location = cp.Location;
            foundContract.Approver1Status = cp.Approver1Status;
            foundContract.Approver2Status = cp.Approver2Status;
            foundContract.Approver3Status = cp.Approver3Status;
            foundContract.IsDeleted = cp.IsDeleted;
            _context.ContractsEntity.Update(foundContract);
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
