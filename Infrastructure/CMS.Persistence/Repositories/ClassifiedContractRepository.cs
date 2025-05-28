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
using CMS.Application.Features.ClassifiedPostTermination.Command.AddCommand;
using CMS.Application.Features.ClassifiedNoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using System.Data;

namespace CMS.Persistence.Repositories
{
    public class ClassifiedContractRepository : IClassifiedContractRepository
    {
        readonly CMSDbContext _context;
        readonly IEmailService _emailService;
        readonly INotificationRepository _notificationRepository;
        IWebHostEnvironment _environment;
        public ClassifiedContractRepository(CMSDbContext context, IWebHostEnvironment environment, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _context = context;
            _environment = environment;

            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }

        public async Task<ContractsCount> GetClassifiedContractsCountAsync()
        {
            string sql = "EXEC SP_ClassifiedContractsCounts";
            var counter = _context.ContractsCounter.FromSqlRaw(sql).AsNoTracking().ToList();
            var allCounters = counter.FirstOrDefault();
            if (allCounters == null)
                throw new NotFoundException("Classified Contracts count not found for some reason");
            return allCounters;
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
        public async Task<ClassifiedContract> AddClassifiedContractAsync(ClassifiedContract cp, string empName)
        {
            if (cp.SkipApproval)
            {
                cp.Approver1Status = ContractStatus.Active;
                cp.Approver2Status = ContractStatus.Active;
                cp.Approver3Status = ContractStatus.Active;
            }
            
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

            string empCode;
            if (foundContract != null)
            {
                empCode = foundContract.Approver1EmployeeCode;
                // To Approver L1
                await AddNewNotifications(empCode,
                                          $"New Classified Contract called '{foundContract.ClassifiedContractName}' Added!",
                                          "New Classified Contract has been added under your department. You can access and change the approvals for this contract.");
                string subject = $"Contract Added";
                string emailBody = string.Empty;
                emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
                emailBody += $"<h1>Hello {foundContract.Approver1EmployeeCode}, new Classified contract has been started under your department.</h1>";
                emailBody += $"<h2>Contract ID: {cp.ClassifiedContractId}<br>Contract Name: {cp.ClassifiedContractName}</h2>";
                emailBody += "<h3>Please check your CMS portal.</h3>";
                emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
                emailBody += "</div>";
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, cp.ClassifiedContractId, cp.ClassifiedContractName, subject, emailBody
                );
                // To Employee Custodian
                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          $"You have added new Classified Contract called '{foundContract.ClassifiedContractName}'!",
                                          "New Classified Contract has been added under your department.");
                emailBody = string.Empty;
                emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
                emailBody += $"<h1>Hello {foundContract.EmpCustodianCode}, new Classified contract has been started under your department.</h1>";
                emailBody += $"<h2>Contract ID: {cp.ClassifiedContractId}<br>Classified Contract Name: {cp.ClassifiedContractName}</h2>";
                emailBody += "<h3>Please check your CMS portal.</h3>";
                emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
                emailBody += "</div>";
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, cp.ClassifiedContractId, cp.ClassifiedContractName, subject, emailBody
                );
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                 await _context.Database.ExecuteSqlRawAsync(query, foundContract.ClassifiedContractId, TableList.ClassifiedContract ," New Classified Contract created by "+empCode, empName , LogStatus.Created);
            }
            return cp;
        }

        public async Task<ClassifiedContract> ApproveRejectContract(int id, string empCode, ContractStatus status)
        {
            var contract = await _context.ClassifiedContracts.Where(c => c.ClassifiedContractId == id).FirstOrDefaultAsync();
            var emp= await _context.MasterEmployees.Where(e=>e.Email ==  empCode).FirstOrDefaultAsync();
            if (contract == null)
            {
                throw new NotFoundException("Classified Contract not found");
            }
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (foundContract == null)
            {
                throw new NotFoundException("Classified Contract not found");
            }
            string approveOrReject = (status == ContractStatus.Active) ? "Approved" : "Rejected";
            string notificationSubject = (status == ContractStatus.Active) ? "Classified Contract has been approved under your department. You can access and change the approvals for this contract." : "Classified Contract has been rejected under your department.";

            if (foundContract.Approver1Email == empCode)
            {

                if (contract.Approver1Status != ContractStatus.PendingApproval)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ClassifiedContractName}({id}),{approveOrReject} by Approver 1";
                contract.Approver1Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not updated");
                }

                await AddNewNotifications(foundContract.Approver2EmployeeCode,
                                          notificationSubject,
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");
                string emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver1EmployeeCode, 1);
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver2EmployeeCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver1EmployeeCode, 1);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );
            }
            else if (foundContract.Approver2Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.Active || contract.Approver2Status != ContractStatus.PendingApproval)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Classified Contract:{foundContract.ClassifiedContractName}({id}),{approveOrReject} by Approver 2";
                contract.Approver2Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not approved");
                }

                await AddNewNotifications(foundContract.Approver3EmployeeCode,
                                          notificationSubject,
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                string emailBody = GenerateEmailBody(foundContract.Approver3EmployeeCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.Approver3Email, foundContract.Approver3EmployeeCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 2.",
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                            $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );
            }
            else if (foundContract.Approver3Email == empCode)
            {
                if (contract.Approver2Status != ContractStatus.Active || contract.Approver1Status != ContractStatus.Active || contract.Approver3Status != ContractStatus.PendingApproval)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ClassifiedContractName}({id}),{approveOrReject} by Approver 3";
                contract.Approver3Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not approved");
                }
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 3.",
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                string emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver2EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 3.",
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver1EmployeeCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                            $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ClassifiedContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, subject, emailBody
                );
            }
            else
            {
                throw new Exception("Unauthorized Action");
            }
            if (status == ContractStatus.Rejected)
            {
                contract.Approver1Status = status;
                contract.Approver2Status = status;
                contract.Approver3Status = status;


                string rejectedQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(rejectedQuery, foundContract.ClassifiedContractId, TableList.ClassifiedContract, " Classified Contract Rejected by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Rejected);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reasons , contract status has not been changed to {status}");
                }
                return contract;
            }

            
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, foundContract.ClassifiedContractId, TableList.ClassifiedContract, " Classified Contract Approved by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Approved)  ;
            
            return contract;
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

        //for creating contract
        private string GenerateEmailBody(string empCode, string contractName, int contractID, string approveOrReject, string approverCode, int approverLevel)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>Hello {empCode},</h1>";
            emailBody += $"<h2>Classified Contract called '{contractName}'({contractID}), {approveOrReject} by '{approverCode}'(Approver {approverLevel})!.</h2>";
            emailBody += "<h3>Please check your CMS portal.</h3>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }


        public async Task SendMail(string email, string name, int contractID, string contractName, string subject, string emailBody)
        {
            var mailRequest = new MailRequest
            {
                Email = email,
                Subject = subject,
                EmailBody = emailBody
            };
            await _emailService.SendEmail(mailRequest);
        }

       //for termination
        private string GenerateEmailBody(string name, int contractID, string contractName)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>NOTICE FOR {name}</h1>";
            emailBody += $"<h2>NOTICE: Withdrawal of Termination for the Classified contract ID {contractID} is initialized. Please check the portal for Approval or Rejection.</h2>";
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

        public async Task<bool> DeleteClassifiedContractAsync(int id,string empCode)
        {
            var foundContract = await _context.ClassifiedContracts.FirstOrDefaultAsync(ce => ce.ClassifiedContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Classified Contract with id {id} not found. Please enter correct id");
            }
            foundContract.IsDeleted = true;
            _context.ClassifiedContracts.Update(foundContract);

            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, foundContract.ClassifiedContractId, TableList.ClassifiedContract, "Classified Contract Deleted by " + empCode, empCode, LogStatus.Deleted);

            if (await _context.SaveChangesAsync() > 0)
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


        public async Task<bool> AddNoticeWithdrawalDetailsAsync(int contractId, int postTermId, NoticeWithdrawalDocumentUploadDto noticeWithdrawalDocumentUploadDto)
        {
            var checkContract = await _context.ClassifiedContracts.FirstOrDefaultAsync(c => c.ClassifiedContractId == contractId);
            if (checkContract == null)
                throw new NotFoundException($"Classified Contract with id {contractId} not found");
            var checkNotice = await _context.ClassifiedPostTerminationNotices.FirstOrDefaultAsync(ptn => ptn.ClassifiedContractId == contractId && ptn.End_Date >= DateTime.Now);
            if (checkNotice == null)
                throw new NotFoundException($"Post Termination Notice with id {postTermId} not found");
            if (checkContract.Approver3Status != ContractStatus.ApprovedForTermination)
            {
                throw new Exception("Cannot withdraw the contract as the Classified contract is not in the 'To Be Terminated' status.");
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

            //saving the file
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await noticeWithdrawalDocumentUploadDto.File.CopyToAsync(stream);
            }

            //creating new db record
            var document = new ClassifiedNoticeWithdrawal
            {
                ClassifiedContractId = contractId,
                TerminationNoticeId = checkNotice.ValueId,
                DocumentPath = FilePath,
                DisplayDocumentName = originalFileName,
                Remark = noticeWithdrawalDocumentUploadDto.Remark

            };

            await _context.ClassifiedNoticeWithdrawals.AddAsync(document);
            var updateStatus = await _context.ClassifiedContracts.FirstOrDefaultAsync(c => c.ClassifiedContractId == document.ClassifiedContractId);
            updateStatus.Approver1Status = ContractStatus.PendingNoticeWithdrawn;
            updateStatus.Approver2Status = ContractStatus.PendingNoticeWithdrawn;
            updateStatus.Approver3Status = ContractStatus.PendingNoticeWithdrawn;
            _context.ClassifiedContracts.Update(updateStatus);
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos.FromSqlRaw(sql, document.ClassifiedContractId).AsNoTracking().ToListAsync();
            var forNotif = findingContract.FirstOrDefault();
            if (_context.SaveChanges() > 0)
            {
                await AddNewNotifications(forNotif.EmpCustodianCode,
                                          $"Notice for Withdrawal of termination notice for Classified contract '{forNotif.ClassifiedContractName}'!",
                                          $"NOTICE: Withdrawal of Termination for the Classified contract ID {forNotif.ClassifiedContractId} is initialized. Please check the portal.");
                await SendMail(forNotif.EmpCustodianEmail,
                               "Withdrawal of Termination Notice Post Contract! 🚨",
                               GenerateEmailBody(forNotif.EmpCustodianCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName),
                               forNotif.EmpCustodianCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName, document.DocumentPath);

                await AddNewNotifications(forNotif.Approver1EmployeeCode,
                                          $"Notice for Withdrawal of termination notice for Classified contract '{forNotif.ClassifiedContractName}'!",
                                          $"NOTICE: Withdrawal of Termination for the Classified contract ID {forNotif.ClassifiedContractId} is initialized. Please check the portal for approval/rejection.");
                await SendMail(forNotif.Approver1Email,
                               "Withdrawal of Termination Notice Post Contract! 🚨",
                               GenerateEmailBody(forNotif.Approver1EmployeeCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName),
                               forNotif.Approver1EmployeeCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName, document.DocumentPath);

                return true;
            }
            else
            {
                throw new Exception("For Some reasons, Document not uploaded");
            }
        }


        public async Task<bool> AddTerminationDetailsAsync(int contractId, TerminationDocumentUploadDto _terminationDocumentUploadDto)
        {
            if (_terminationDocumentUploadDto.End_Date < DateTime.Now)
            {
                throw new Exception($"End date cant be smaller than current date !");
            }
            var checkContract = await _context.ClassifiedContracts.FirstOrDefaultAsync(c => c.ClassifiedContractId == contractId);
            if (checkContract == null)
                throw new NotFoundException($"Classified Contract with id {contractId} not found");
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

            //saving the file
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await _terminationDocumentUploadDto.File.CopyToAsync(stream);
            }

            //creating new db record
            var document = new ClassifiedPostTerminationNotice
            {
                ClassifiedContractId = contractId,
                DocumentPath = FilePath,
                DisplayDocumentName = originalFileName,
                Notice_Duration = _terminationDocumentUploadDto.Notice_Duration,
                Remark = _terminationDocumentUploadDto.Remark,
                End_Date = _terminationDocumentUploadDto.End_Date

            };

            await _context.ClassifiedPostTerminationNotices.AddAsync(document);
            var updateStatus = await _context.ClassifiedContracts.FirstOrDefaultAsync(c => c.ClassifiedContractId == document.ClassifiedContractId);
            updateStatus.Approver1Status = ContractStatus.PendingTermination;
            updateStatus.Approver2Status = ContractStatus.PendingTermination;
            updateStatus.Approver3Status = ContractStatus.PendingTermination;
            _context.ClassifiedContracts.Update(updateStatus);
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos.FromSqlRaw(sql, document.ClassifiedContractId).AsNoTracking().ToListAsync();
            var forNotif = findingContract.FirstOrDefault();
            if (_context.SaveChanges() > 0)
            {
                await AddNewNotifications(forNotif.EmpCustodianCode,
                                          $"Notice for Termination of Classified contract '{forNotif.ClassifiedContractName}'!",
                                          $"NOTICE: Termination for the contract ID {forNotif.ClassifiedContractId} is initialized. Please check the portal.");
                await SendMail(
                    forNotif.EmpCustodianEmail, "Termination Notice Post Contract! 🚨", GenerateEmailBody(forNotif.EmpCustodianCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName), forNotif.EmpCustodianCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName, document.DocumentPath
                );
                await AddNewNotifications(forNotif.Approver1EmployeeCode,
                                          $"Notice for Termination of contract '{forNotif.ClassifiedContractName}'!",
                                          $"NOTICE: Termination for the contract ID {forNotif.ClassifiedContractId} is initialized. Please check the portal.");
                await SendMail(
                    forNotif.Approver1Email, "Termination Notice Post Contract! 🚨", GenerateEmailBody(forNotif.Approver1EmployeeCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName), forNotif.Approver1EmployeeCode, forNotif.ClassifiedContractId, forNotif.ClassifiedContractName, document.DocumentPath
                );

              
                return true;
            }
            else
            {
                throw new Exception("For Some reasons, Document not uploaded");
            }


        }

        public async Task<ClassifiedContract> ApproveTerminateContract(int id, string empCode, ContractStatus status, string subject, string emailBody)
        {
            var contract = await _context.ClassifiedContracts.Where(c => c.ClassifiedContractId == id).FirstOrDefaultAsync();
            var forDoc = await _context.ClassifiedPostTerminationNotices.FirstOrDefaultAsync(c => c.ClassifiedContractId == id);
            var emp = await _context.MasterEmployees.Where(e => e.Email == empCode).FirstOrDefaultAsync();

            if (contract == null)
            {
                throw new NotFoundException("Classified Contract not found");
            }
            if (forDoc == null)
            {
                throw new NotFoundException($"Post Termination Notice not found for contract id {id}");
            }
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();

            if (foundContract == null)
            {
                throw new NotFoundException("Contract Not Found");
            }

            string approveOrTerminate = (status == ContractStatus.ApprovedForTermination) ? "Approved" : "Terminated";
            string notificationSubject = (status == ContractStatus.ApprovedForTermination) ? "Classified Contract has been approved under your department. You can access and change the approvals for this contract." : "Classified Contract has been Terminated under your department.";

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

                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"Classified contract called'{foundContract.ClassifiedContractName} {approveOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
                    );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}'{approveOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
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

                await AddNewNotifications(foundContract.Approver3EmployeeCode, notificationSubject, $"Classified Contract called '{foundContract.ClassifiedContractName}' " +
                    $"{approveOrTerminate} by {foundContract.Approver2EmployeeCode}'" +
                    $"(Approver 2)!");

                await SendMail(foundContract.Approver3Email, subject, emailBody, foundContract.Approver3EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
                   );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                    $"Classified Contract called '{foundContract.ClassifiedContractName}' {approveOrTerminate} by '{foundContract.Approver2EmployeeCode}' (Approver 2)!");


                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
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
                    throw new Exception("For some reaons , contract is not approved");
                }

                //to approver 1
                await AddNewNotifications(foundContract.Approver1EmployeeCode, notificationSubject, $"Classified Contract Called '{foundContract.ClassifiedContractName}' " +
                    $"{approveOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                    $"(Approver 3)!");

                await SendMail(foundContract.Approver1Email, subject, emailBody, foundContract.Approver1EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath);

                //to approver 2
                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"Classified Contract Called '{foundContract.ClassifiedContractName}' " +
                   $"{approveOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                   $"(Approver 3)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath);

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                         $"Classified Contract called '{foundContract.ClassifiedContractName}'{approveOrTerminate} by '{foundContract.Approver2EmployeeCode}'(Approver 3)!");


                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
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
                await _context.Database.ExecuteSqlRawAsync(rejectedQuery, foundContract.ClassifiedContractId, TableList.ClassifiedContract, "classified contract "+foundContract.ClassifiedContractName +" Termination Request has been Rejected by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Rejected);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reaons , contract status has not been changed to {status}");
                }
                return contract;
            }

            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, foundContract.ClassifiedContractId, TableList.ClassifiedContract, " Classified Contract "+foundContract.ClassifiedContractName+", Approved for Termination by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Terminated);

            return contract;

        }

        public async Task<ClassifiedContract> ApproveNoticeWithdrawalAsync(int id, string empCode, ContractStatus status, string subject, string emailBody)
        {
            var contract = await _context.ClassifiedContracts.Where(c => c.ClassifiedContractId == id).FirstOrDefaultAsync();
            var forDoc = await _context.ClassifiedNoticeWithdrawals.FirstOrDefaultAsync(c => c.ClassifiedContractId == id);
            var emp = await _context.MasterEmployees.Where(e => e.Email == empCode).FirstOrDefaultAsync();


            if (contract == null)
            {
                throw new NotFoundException("Classified Contract not found");
            }
            if (forDoc == null)
            {
                throw new NotFoundException($"Notice Withdrawal not found for contract id {id}");
            }
            string sql = "EXEC SP_GetClassifiedContractByID @ID = {0}";
            var findingContract = await _context.GetClassifiedContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();

            if (foundContract == null)
            {
                throw new NotFoundException("Classified Contract Not Found");
            }

            string activeOrTerminate = (status == ContractStatus.Active) ? "Withdrawn" : "Terminated";
            string notificationSubject = (status == ContractStatus.Active) ? $"Withdrawal notice for the contract id {id} has been approved under your department. Contract's status is back to active" : "Classified Contract has been Terminated under your department.";

            if (foundContract.Approver1Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.PendingNoticeWithdrawn)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver1Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons contract status has not been updated");
                }

                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"contract called'{foundContract.ClassifiedContractName} {activeOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
                    );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                          $"Classified Contract called '{foundContract.ClassifiedContractName}'{activeOrTerminate} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
                    );

            }
            else if (foundContract.Approver2Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.Active || contract.Approver2Status != ContractStatus.PendingNoticeWithdrawn)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver2Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, Contract status has not been approved.");
                }

                await AddNewNotifications(foundContract.Approver3EmployeeCode, notificationSubject, $"Contract called '{foundContract.ClassifiedContractName}' " +
                    $"{activeOrTerminate} by {foundContract.Approver2EmployeeCode}'" +
                    $"(Approver 2)!");

                await SendMail(foundContract.Approver3Email, subject, emailBody, foundContract.Approver3EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
                   );

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                    $"Contract called '{foundContract.ClassifiedContractName}' {activeOrTerminate} by '{foundContract.Approver2EmployeeCode}' (Approver 2)!");


                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
                    );
            }
            else if (foundContract.Approver3Email == empCode)
            {
                if (contract.Approver2Status != ContractStatus.Active || contract.Approver1Status != ContractStatus.Active || contract.Approver3Status != ContractStatus.PendingNoticeWithdrawn)
                {
                    throw new Exception("Invalid approval action");
                }

                contract.Approver3Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reaons , Contract status has not been approved.");
                }

                //to approver 1
                await AddNewNotifications(foundContract.Approver1EmployeeCode, notificationSubject, $"Contract Called '{foundContract.ClassifiedContractName}' " +
                    $"{activeOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                    $"(Approver 3)!");

                await SendMail(foundContract.Approver1Email, subject, emailBody, foundContract.Approver1EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath);

                //to approver 2
                await AddNewNotifications(foundContract.Approver2EmployeeCode, notificationSubject, $"Contract Called '{foundContract.ClassifiedContractName}' " +
                   $"{activeOrTerminate} by {foundContract.Approver3EmployeeCode}'" +
                   $"(Approver 3)!");

                await SendMail(foundContract.Approver2Email, subject, emailBody, foundContract.Approver2EmployeeCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath);

                await AddNewNotifications(foundContract.EmpCustodianCode, notificationSubject.Split('.')[0] + "'",
                                         $"Contract called '{foundContract.ClassifiedContractName}'{activeOrTerminate} by '{foundContract.Approver2EmployeeCode}'(Approver 3)!");


                await SendMail(foundContract.EmpCustodianEmail, subject, emailBody, foundContract.EmpCustodianCode, id, foundContract.ClassifiedContractName, forDoc.DocumentPath
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

                string rejectedQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(rejectedQuery, foundContract.ClassifiedContractId, TableList.ClassifiedContract, " Classified Contract " + foundContract.ClassifiedContractName + ", Rejected for Withdrawal Notice by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Rejected);


                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reaons , contract status has not been changed to {status}");
                }
            return contract;

            }

            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, foundContract.ClassifiedContractId, TableList.ClassifiedContract, " Classified Contract " + foundContract.ClassifiedContractName + ", Approved for Withdrawal Notice by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Approved);

            return contract;
        }
    }
}
