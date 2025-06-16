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
            if (_terminationDocumentUploadDto.End_Date<DateTime.Now)
            {
                throw new Exception($"End date cant be smaller than current date !");
            }
            var checkContract = await _context.ContractsEntity.FirstOrDefaultAsync(c => c.ContractId == contractId);
            if (checkContract == null)
                throw new NotFoundException($"Contract with id {contractId} not found");
            if (checkContract.Approver3Status != ContractStatus.Active)
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
            if (_context.SaveChanges() > 0)
            {
                await AddNewNotifications(forNotif.EmpCustodianCode,
                                          $"Notice for Termination of contract '{forNotif.ContractName}'!",
                                          $"NOTICE: Termination for the contract ID {forNotif.ContractId} is initialized. Please check the portal.");
                await SendMail(
                    forNotif.EmpCustodianEmail, "Termination Notice Post Contract! 🚨", GenerateEmailBody(forNotif.EmpCustodianCode, forNotif.ContractId, forNotif.ContractName), forNotif.EmpCustodianCode, forNotif.ContractId, forNotif.ContractName, document.DocumentPath
                );
                await AddNewNotifications(forNotif.Approver1EmployeeCode,
                                          $"Notice for Termination of contract '{forNotif.ContractName}'!",
                                          $"NOTICE: Termination for the contract ID {forNotif.ContractId} is initialized. Please check the portal.");
                await SendMail(
                    forNotif.Approver1Email, "Termination Notice Post Contract! 🚨", GenerateEmailBody(forNotif.Approver1EmployeeCode, forNotif.ContractId, forNotif.ContractName), forNotif.Approver1EmployeeCode, forNotif.ContractId, forNotif.ContractName, document.DocumentPath
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
            emailBody += $"<h2>NOTICE: Termination for the contract ID {contractID} is initialized. Please check the portal for Approval or Rejection.</h2>";
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

        public async Task<Contract> ApproveTerminateContract(int id, string empCode, ContractStatus status, string subject, string emailBody)
        {
            var contract = await _context.ContractsEntity.Where(c => c.ContractId == id).FirstOrDefaultAsync();
            var forDoc = await _context.PostTerminationNotices.FirstOrDefaultAsync(c => c.ContractId == id);
            var emp = await _context.MasterEmployees.Where(e => e.Email == empCode).FirstOrDefaultAsync();

            if (contract == null)
            {
                throw new NotFoundException("Contract not found");
            }
            if (forDoc == null)
            {
                throw new NotFoundException($"Post Termination Notice not found for contract id {id}");
            }
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();

            if (foundContract == null)
            {
                throw new NotFoundException("Contract Not Found");
            }

            string approveOrTerminate = (status == ContractStatus.ApprovedForTermination) ? "Approved" : "Terminated";
            string notificationSubject = (status == ContractStatus.ApprovedForTermination) ? "Contract has been approved under your department. You can access and change the approvals for this contract." : "Contract has been Terminated under your department.";

            if (foundContract.Approver1Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.PendingTermination)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver1Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons contract is not updated");
                }

                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"contract called'{foundContract.ContractName} {approveOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                          $"Contract called '{foundContract.ContractName}'{approveOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );

            }
            else if (foundContract.Approver2Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.ApprovedForTermination || contract.Approver2Status != ContractStatus.PendingTermination)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver2Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, Contract is not approved");
                }

                await AddNewNotifications(foundContract.Approver3EmployeeCode, notificationSubject, $"Contract called '{foundContract.ContractName}' " +
                    $"{approveOrTerminate} by {foundContract.Approver2EmployeeCode}'" +
                    $"(Approver 2)!");

                await SendMail(foundContract.Approver3Email, subject, emailBody, foundContract.Approver3EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath
                   );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                    $"Contract called '{foundContract.ContractName}' {approveOrTerminate} by '{foundContract.Approver2EmployeeCode}' (Approver 2)!");


                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );
            }
            else if (foundContract.Approver3Email == empCode)
            {
                if (contract.Approver2Status != ContractStatus.ApprovedForTermination || contract.Approver1Status != ContractStatus.ApprovedForTermination || contract.Approver3Status != ContractStatus.PendingTermination)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver3Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons , contract is not approved");
                }

                //to approver 1
                await AddNewNotifications(foundContract.Approver1EmployeeCode, notificationSubject, $"Contract Called '{foundContract.ContractName}' " +
                    $"{approveOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                    $"(Approver 3)!");

                await SendMail(foundContract.Approver1Email, subject, emailBody, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath);

                //to approver 2
                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"Contract Called '{foundContract.ContractName}' " +
                   $"{approveOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                   $"(Approver 3)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ContractName, forDoc.DocumentPath);

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                         $"Contract called '{foundContract.ContractName}'{approveOrTerminate} by '{foundContract.Approver2EmployeeCode}'(Approver 3)!");


                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ContractName, forDoc.DocumentPath
                    );

            }
            else
            {
                throw new Exception("Unauthorized Action");
            }
            if (status == ContractStatus.Active)
            {
                contract.Approver1Status = status;
                contract.Approver2Status = status;
                contract.Approver3Status = status;

                string rejectedQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(rejectedQuery, foundContract.ContractId, TableList.Contract, $"Contract '{foundContract.ContractName}' Termination Request has been Rejected by '{emp.EmployeeCode}'", emp.EmployeeCode, LogStatus.Rejected);


                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reaons , contract status has not been changed to {status}");
                }
                return contract;
            }


            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, foundContract.ContractId, TableList.Contract, $" Contract '{ foundContract.ContractName }', Approved for Termination by {emp.EmployeeCode}'", emp.EmployeeCode, LogStatus.Terminated);


            return contract;

        }


    }
}
