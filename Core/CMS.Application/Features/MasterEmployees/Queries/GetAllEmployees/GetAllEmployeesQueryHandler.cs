using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, object>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;
        private readonly ICacheService _cacheService;

        public GetAllEmployeesQueryHandler(IMasterEmployeeRepository masterEmployeeRepository, ICacheService cacheService)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
            _cacheService = cacheService;
        }

        public async Task<object> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"Employees_{request.pageNumber}_{request.pageSize}_{request.unit}_{request.searchTerm}";

            ////getting from cache
            //var cachedEmployees = await _cacheService.GetAsync<(IEnumerable<MasterEmployee> Data, int TotalCount)>(cacheKey);

            //if (cachedEmployees.Data != null && cachedEmployees.TotalCount != null)
            //{
            //    return new
            //    {
            //        data = cachedEmployees.Data,
            //        totalCount = cachedEmployees.TotalCount
            //    };
            //}

            ////not in cache then fetching from repo
            //var employees = await _masterEmployeeRepository.GetAllEmployeesAsync(request.pageNumber, request.pageSize, request.unit, request.searchTerm);

            ////storing in cache
            //await _cacheService.SetAsync(cacheKey, employees, TimeSpan.FromMinutes(1));



            var result = await _masterEmployeeRepository.GetAllEmployeesAsync(request.pageNumber, request.pageSize, request?.unit, request?.searchTerm);
            return new
            {
                data = result.Data,
                totalCount = result.TotalCount
            };
        }
    }
}
