using CMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Persistence.Repositories
{
    public class CacheWarmupHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public CacheWarmupHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 1 for All Companies
            using (var scope = _serviceProvider.CreateScope())
            {

                var repo = scope.ServiceProvider.GetRequiredService<IMasterCompanyRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                string searchTerm = "";
                int pageNumber = 1;
                int pageSize = 100;

                var companies = await repo.GetAllCompanyDetailsAsync(searchTerm, pageNumber, pageSize);
                string cacheKey = $"companies_{searchTerm}_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, companies, TimeSpan.FromMinutes(5));
            }

            //2 for All Contract Types
            using (var scope= _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IContractTypeMasterRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                int pageNumber = 1;
                int pageSize = 100;

                var contracts = await repo.GetAllContractAsync(pageNumber, pageSize);

                string cacheKey = $"contracts_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, contracts, TimeSpan.FromMinutes(5));

            }
            //3 for All Employees
            using(var scope= _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IMasterEmployeeRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                int pageNumber = 1;
                int pageSize = 100;
                string unit = "";
                string searchTerm = "";

                var employees = await repo.GetAllEmployeesAsync(pageNumber, pageSize, unit, searchTerm);

                string cacheKey = $"emplpoyees_{pageNumber}_{pageSize}_{unit}_{searchTerm}";

                await cache.SetAsync(cacheKey, employees, TimeSpan.FromMinutes(5));

            }

            //4 for All Apostilles
            using(var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IMasterApostilleRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                int pageNumber = 1;
                int pageSize = 100;
                string searchTerm = "";

                var Apostilles = await repo.GetAllMasterApostilleAsync(pageNumber, pageSize, searchTerm);

                string cacheKey = $"Apostille_{pageNumber}_{pageSize}_{searchTerm}";

                await cache.SetAsync(cacheKey, Apostilles, TimeSpan.FromMinutes(5));
            }

            //5 for all documents
            using(var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IDocumentRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                int pageNumber = 1;
                int pagesize = 100;

                var docum = await repo.GetAllDocuments(pageNumber, pagesize);

                string cacheKey = $"Documents_{pageNumber}_{pagesize}";

                await cache.SetAsync(cacheKey, docum, TimeSpan.FromMinutes(5));
            }

            //6 for all EscalationMatricContract
            using(var scope=_serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IMasterEscalationMatrixContractRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                int pageNumber = 1;
                int pageSize = 100;

                var contrr = await repo.GetAllEscalationMatrixContract(pageNumber, pageSize);

                string cacheKey = $"Contract_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, contrr, TimeSpan.FromMinutes(5));
            }

            //7 for all Departments 
            using(var scope =_serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IDepartmentRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload 
                int pageNumber = 1;
                int pageSize = 100;

                var depttt = await repo.GetAllDepartments(pageNumber, pageSize);

                string cacheKey = $"Departement_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, depttt, TimeSpan.FromMinutes(5));
            }

            //8 for getting allMatrixMou
            using(var scope= _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IMasterApprovalMatrixMOURepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload
                int pageNumber = 1;
                int pageSize = 10;

                var AllMatrixmou = await repo.GetAllApprovalMatrixMOU(pageNumber, pageSize);

                string cacheKey = $"AllMatrixMou_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, AllMatrixmou, TimeSpan.FromMinutes(5));
            }

            //9 for getting all ApprovalMatrixContract
            using(var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IMasterApprovalMatrixContractRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload

                int pageNumber = 1;
                int pageSize = 10;

                var allContractMou = await repo.GetAllApprovalMatrixContract(pageNumber, pageSize);

                string cacheKey = $"AllMatrixMou_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, allContractMou, TimeSpan.FromMinutes(5));
            }

            //10 for getting all EscalationMatrixMou
            using(var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IMasterEscalationMatrixMouRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();

                //defining preload

                int pageNumber = 1;
                int pageSize = 10;

                var allEscalationMatrixMou = await repo.GetAllEscalationMatrixMou(pageNumber, pageSize);

                string cacheKey = $"AllEscalationMatrixMou_{pageNumber}_{pageSize}";

                await cache.SetAsync(cacheKey, allEscalationMatrixMou, TimeSpan.FromMinutes(5));
            }


        }


        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
       
    }
}
