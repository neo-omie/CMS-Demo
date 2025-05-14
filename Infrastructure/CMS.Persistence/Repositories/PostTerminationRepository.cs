using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Application.Features.PostTermination.Command.AddCommand;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Persistence.Repositories
{
    public class PostTerminationRepository : IPostTerminationRepository
    {
        private readonly CMSDbContext _context;

        private readonly IWebHostEnvironment _environment;

        readonly IEmailService _emailService;
        readonly INotificationRepository _notificationRepository;
        public PostTerminationRepository(CMSDbContext context, IWebHostEnvironment environment, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _context = context;
            _environment = environment;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }
        public async Task<bool> AddTerminationDetailsAsync(int contractId, TerminationDocumentUploadDto _terminationDocumentUploadDto)
        {
            var checkContract = await _context.ContractsEntity.FirstOrDefaultAsync(c => c.ContractId == contractId);
            if (checkContract == null)
                throw new NotFoundException($"Contract with id {contractId} not found");
            if(checkContract.Approver3Status != ContractStatus.Active)
            {
                throw new Exception("Cannot terminate the contract as the contract has not been active.");
            }
            var allowedExtesnisons = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(_terminationDocumentUploadDto.File.FileName).ToLowerInvariant();

            if (!allowedExtesnisons.Contains(fileExtension))
            {
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png.");

            }
            if (_terminationDocumentUploadDto.File == null || _terminationDocumentUploadDto.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            const long maxFileSize = 25 * 1024 * 1024;

            if (_terminationDocumentUploadDto.File.Length > maxFileSize)
            {
                throw new Exception("File size Exceeds the 25 mb limit .");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var originalFileName = Path.GetFileName(_terminationDocumentUploadDto.File.FileName);
            var FilePath = Path.Combine(uploadsFolder, originalFileName);

            //savig the file
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await _terminationDocumentUploadDto.File.CopyToAsync(stream);
            }

            //creating new db record
            var document = new PostTerminationNotice
            {
                ContractId = contractId,
                DocumentPath = FilePath,
                DisplayDocumentName = originalFileName,
                Notice_Duration = _terminationDocumentUploadDto.Notice_Duration,
                Remark = _terminationDocumentUploadDto.Remark,
                End_Date = _terminationDocumentUploadDto.End_Date 

            };

            await _context.PostTerminationNotices.AddAsync(document);
            var updateStatus = await _context.ContractsEntity.FirstOrDefaultAsync(c => c.ContractId == document.ContractId);
            updateStatus.Approver1Status = ContractStatus.PendingTermination;
            updateStatus.Approver2Status = ContractStatus.PendingTermination;
            updateStatus.Approver3Status = ContractStatus.PendingTermination;
            _context.ContractsEntity.Update(updateStatus);
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, document.ContractId).AsNoTracking().ToListAsync();
            var forNotif = findingContract.FirstOrDefault();
            if ( _context.SaveChanges()>0)
            {
                await AddNewNotifications(forNotif.EmpCustodianCode,
                                          $"Notice for Termination of contract '{forNotif.ContractName}'!",
                                          $"NOTICE: Termination for the contract ID {forNotif.ContractId} is initialized. Please check the portal.");
                await SendMail(
                    forNotif.EmpCustodianEmail, forNotif.EmpCustodianCode, forNotif.ContractId, forNotif.ContractName,document.DocumentPath
                );
                await AddNewNotifications(forNotif.Approver1EmployeeCode,
                                          $"Notice for Termination of contract '{forNotif.ContractName}'!",
                                          $"NOTICE: Termination for the contract ID {forNotif.ContractId} is initialized. Please check the portal.");
                await SendMail(
                    forNotif.Approver1Email, forNotif.Approver1EmployeeCode, forNotif.ContractId, forNotif.ContractName, document.DocumentPath
                );

                return true;
            }
            else
            {
                throw new Exception("For Some reasons, Document not uploaded");
            }

                
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
            var existing = _context.ChangeTracker.Entries<Notification>().FirstOrDefault(e => e.Entity.EmployeeCode == name);

            if (existing != null)
            {
                existing.State = EntityState.Detached;
            }

        }
        private string GenerateEmailBody(string name, int contractID, string contractName)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>NOTICE FOR {name}</h1>";
            emailBody += $"<h2>NOTICE: Termination for the contract ID {contractID} is initialized. Please check the portal.</h2>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }
        public async Task SendMail(string email, string name, int contractID, string contractName,string attachmentpath)
        {
            var mailRequest = new MailRequestWithAttachment
            {
                Email = email,
                Subject = "Termination Notice Post Contract! 🚨",
                EmailBody = GenerateEmailBody(name, contractID, contractName),
                Attachments=attachmentpath
            };
            await _emailService.SendEmailWithAttachment(mailRequest);
        }


    }
}
