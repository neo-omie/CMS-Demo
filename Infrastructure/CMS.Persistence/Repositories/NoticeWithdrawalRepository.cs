using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Application.Features.NoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class NoticeWithdrawalRepository : INoticeWithdrawalRepository
    {
        readonly CMSDbContext _context;
        private readonly IWebHostEnvironment _environment;
        readonly IEmailService _emailService;
        readonly INotificationRepository _notificationRepository;
        public NoticeWithdrawalRepository(CMSDbContext context, IWebHostEnvironment environment, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _context = context;
            _environment = environment;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }
        public async Task<bool> AddNoticeWithdrawalDetailsAsync(int contractId, int postTermId, NoticeWithdrawalDocumentUploadDto noticeWithdrawalDocumentUploadDto)
        {
            var checkContract = await _context.ContractsEntity.FirstOrDefaultAsync(c => c.ContractId == contractId);
            if (checkContract == null)
                throw new NotFoundException($"Contract with id {contractId} not found");
            var checkNotice = await _context.PostTerminationNotices.FirstOrDefaultAsync(ptn => ptn.ContractId == contractId && ptn.End_Date >= DateTime.Now);
            if(checkNotice == null)
                throw new NotFoundException($"Post Termination Notice with id {postTermId} not found");
            if (checkContract.Approver3Status != ContractStatus.ApprovedForTermination)
            {
                throw new Exception("Cannot withdraw the contract as the contract is not in the 'To Be Terminated' status.");
            }
            var allowedExtesnisons = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(noticeWithdrawalDocumentUploadDto.File.FileName).ToLowerInvariant();

            if (!allowedExtesnisons.Contains(fileExtension))
            {
                throw new Exception("Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg and .png.");

            }
            if (noticeWithdrawalDocumentUploadDto.File == null || noticeWithdrawalDocumentUploadDto.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            const long maxFileSize = 25 * 1024 * 1024;

            if (noticeWithdrawalDocumentUploadDto.File.Length > maxFileSize)
            {
                throw new Exception("File size Exceeds the 25 mb limit .");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var originalFileName = Path.GetFileName(noticeWithdrawalDocumentUploadDto.File.FileName);
            var FilePath = Path.Combine(uploadsFolder, originalFileName);

            //savig the file
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await noticeWithdrawalDocumentUploadDto.File.CopyToAsync(stream);
            }

            //creating new db record
            var document = new NoticeWithdrawal
            {
                ContractId = contractId,
                TerminationNoticeId = checkNotice.ValueId,
                DocumentPath = FilePath,
                DisplayDocumentName = originalFileName,
                Remark = noticeWithdrawalDocumentUploadDto.Remark

            };

            await _context.NoticeWithdrawals.AddAsync(document);
            var updateStatus = await _context.ContractsEntity.FirstOrDefaultAsync(c => c.ContractId == document.ContractId);
            updateStatus.Approver1Status = ContractStatus.PendingNoticeWithdrawn;
            updateStatus.Approver2Status = ContractStatus.PendingNoticeWithdrawn;
            updateStatus.Approver3Status = ContractStatus.PendingNoticeWithdrawn;
            _context.ContractsEntity.Update(updateStatus);
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, document.ContractId).AsNoTracking().ToListAsync();
            var forNotif = findingContract.FirstOrDefault();
            if (_context.SaveChanges() > 0)
            {
                await AddNewNotifications(forNotif.EmpCustodianCode,
                                          $"Notice for Withdrawal of termination notice for contract '{forNotif.ContractName}'!",
                                          $"NOTICE: Withdrawal of Termination for the contract ID {forNotif.ContractId} is initialized. Please check the portal.");
                await SendMail(forNotif.EmpCustodianEmail,
                               "Withdrawal of Termination Notice Post Contract! 🚨",
                               GenerateEmailBody(forNotif.EmpCustodianCode, forNotif.ContractId, forNotif.ContractName),
                               forNotif.EmpCustodianCode, forNotif.ContractId, forNotif.ContractName, document.DocumentPath);

                await AddNewNotifications(forNotif.Approver1EmployeeCode,
                                          $"Notice for Withdrawal of termination notice for contract '{forNotif.ContractName}'!",
                                          $"NOTICE: Withdrawal of Termination for the contract ID {forNotif.ContractId} is initialized. Please check the portal for approval/rejection.");
                await SendMail(forNotif.Approver1Email,
                               "Withdrawal of Termination Notice Post Contract! 🚨",
                               GenerateEmailBody(forNotif.Approver1EmployeeCode, forNotif.ContractId, forNotif.ContractName),
                               forNotif.Approver1EmployeeCode, forNotif.ContractId, forNotif.ContractName, document.DocumentPath);

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
            emailBody += $"<h2>NOTICE: Withdrawal of Termination for the contract ID {contractID} is initialized. Please check the portal for Approval or Rejection.</h2>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }
        public async Task SendMail(string email, string subject, string emailBody, string name, int contractID, string contractName, string attachmentpath)
        {
            var mailRequest = new MailRequestWithAttachment
            {
                Email = email,
                Subject = subject,
                EmailBody = emailBody,
                Attachments = attachmentpath
            };
            await _emailService.SendEmailWithAttachment(mailRequest);
        }




        public async Task<Contract> ApproveNoticeWithdrawalAsync(int id, string empCode, ContractStatus status, string subject, string emailBody)
        {
            var contract = await _context.ContractsEntity.Where(c => c.ContractId == id).FirstOrDefaultAsync();
            var forDoc = await _context.NoticeWithdrawals.FirstOrDefaultAsync(c => c.ContractId == id);
            
            if (contract ==null)
            {
                throw new NotFoundException("Contract not found");
            }
            if(forDoc == null)
            {
                throw new NotFoundException($"Notice Withdrawal not found for contract id {id}");
            }
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();

            if (foundContract==null)
            {
                throw new NotFoundException("Contract Not Found");
            }

            string activeOrTerminate = (status == ContractStatus.Active) ? "Withdrawn" : "Terminated";
            string notificationSubject=(status==ContractStatus.Active)? $"Withdrawal notice for the contract id {id} has been approved under your department. Contract's status is back to active" : "Contract has been Terminated under your department.";

            if (foundContract.Approver1Email==empCode)
            {
                if (contract.Approver1Status!=ContractStatus.PendingNoticeWithdrawn)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver1Status = status;

                if (await _context.SaveChangesAsync()<=0)
                {
                    throw new Exception ("For some reasons contract status has not been updated");
                }

                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"contract called'{foundContract.ContractName} {activeOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                          $"Contract called '{foundContract.ContractName}'{activeOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");                                                
                
                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );

            }
            else if (foundContract.Approver2Email == empCode)
            {
                if(contract.Approver1Status!=ContractStatus.Active || contract.Approver2Status != ContractStatus.PendingNoticeWithdrawn)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver2Status = status;

                if (await _context.SaveChangesAsync()<=0)
                {
                    throw new Exception("For some reasons, Contract status has not been approved.");
                }

                await AddNewNotifications(foundContract.Approver3EmployeeCode, notificationSubject, $"Contract called '{foundContract.ContractName}' " +
                    $"{activeOrTerminate} by {foundContract.Approver2EmployeeCode}'" +
                    $"(Approver 2)!");

                await SendMail(foundContract.Approver3Email, subject, emailBody, foundContract.Approver3EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath
                   );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                    $"Contract called '{foundContract.ContractName}' {activeOrTerminate} by '{foundContract.Approver2EmployeeCode}' (Approver 2)!");

                
                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );
            }
            else if(foundContract.Approver3Email==empCode)
            {
                if (contract.Approver2Status != ContractStatus.Active || contract.Approver1Status!=ContractStatus.Active || contract.Approver3Status != ContractStatus.PendingNoticeWithdrawn)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver3Status = status;

                if (await _context.SaveChangesAsync()<=0)
                {
                    throw new Exception("For some reaons , Contract status has not been approved.");
                }

                //to approver 1
                await AddNewNotifications(foundContract.Approver1EmployeeCode, notificationSubject, $"Contract Called '{foundContract.ContractName}' " +
                    $"{activeOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                    $"(Approver 3)!");

                await SendMail(foundContract.Approver1Email, subject, emailBody, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath);

                //to approver 2
                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"Contract Called '{foundContract.ContractName}' " +
                   $"{activeOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                   $"(Approver 3)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath);

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                         $"Contract called '{foundContract.ContractName}'{activeOrTerminate} by '{foundContract.Approver2EmployeeCode}'(Approver 3)!");                                              

                
                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );

            }
            else
            {
                throw new Exception("Unauthorized Action");
            }
            if (status == ContractStatus.ApprovedForTermination)
            {
                contract.Approver1Status = status;
                contract.Approver2Status = status;
                contract.Approver3Status = status;
                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reaons , contract status has not been changed to {status}");
                }
            }
            return contract;
        }
    }
}
