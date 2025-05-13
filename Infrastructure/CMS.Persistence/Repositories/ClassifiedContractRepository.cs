using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class ClassifiedContractRepository : IClassifiedContractRepository
    {
        readonly CMSDbContext _context;
        readonly IEmailService _emailService;
        readonly INotificationRepository _notificationRepository;

        public ClassifiedContractRepository(CMSDbContext context, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _context = context;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<GetAllClassifiedContractsDto>> GetAllClassifiedContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ClassifiedContracts.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetAllClassifiedContracts @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetClassifiedContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }

        public async Task<GetClassifiedContractByIdDto> GetClassifiedContractByIdAsync(int id)
        {
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (foundContract == null)
            {
                throw new NotFoundException($"Classified Contract with ID {id} not found");
            }
            return foundContract;

        }
        public async Task<ClassifiedContract> AddClassifiedContractAsync(ClassifiedContract cp)
        {
            if (cp.SkipApproval)
            {
                cp.Approver1Status = ContractStatus.Active;
                cp.Approver2Status = ContractStatus.Active;
                cp.Approver3Status = ContractStatus.Active;
            }
            // Set the creator of the contract
            cp.CreatedBy = "Admin@cms.com";
            // Add the contract to the context
          
            var addedContract =await  _context.ClassifiedContracts.AddAsync(cp);

            if (await _context.SaveChangesAsync() <= 0)
            {
                throw new Exception("For some reasons, contract has not been added.");
            }
            // Retrieve the added contract by ID
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos
                .FromSqlRaw(sql, cp.ClassifiedContractId)
                .AsNoTracking()
                .ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (foundContract != null)
            {
                // To Approver L1
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          $"New Contract called '{foundContract.ClassifiedContractName}' Added!",
                                          "New Contract has been added under your department. You can access and change the approvals for this contract.");
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, cp.ClassifiedContractId, cp.ClassifiedContractName
                );
                // To Employee Custodian
                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          $"You have added new Contract called '{foundContract.ClassifiedContractName}'!",
                                          "New Contract has been added under your department.");
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, cp.ClassifiedContractId, cp.ClassifiedContractName
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
            emailBody += $"<h1>Hello {name}, new classified contract has been started under your department.</h1>";
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
                Subject = "New Classified Contract Added",
                EmailBody = GenerateEmailBody(name, contractID, contractName)
            };
            await _emailService.SendEmail(mailRequest);
        }
        public async Task<bool> DeleteClassifiedContractAsync(int id)
        {
            var foundContract = await _context.ClassifiedContracts.FirstOrDefaultAsync(ce => ce.ClassifiedContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Classified Contract with id {id} not found. Please enter correct id");
            }
            foundContract.IsDeleted = true;
            _context.ClassifiedContracts.Update(foundContract);
            if(await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }


        public async Task<bool> UpdateClassifiedContractAsync(int id, ClassifiedContract cp)
        {
            var foundContract = await _context.ClassifiedContracts.FirstOrDefaultAsync(ce => ce.ClassifiedContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            cp.ClassifiedContractId = foundContract.ClassifiedContractId;
            _context.ClassifiedContracts.Update(cp);
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
