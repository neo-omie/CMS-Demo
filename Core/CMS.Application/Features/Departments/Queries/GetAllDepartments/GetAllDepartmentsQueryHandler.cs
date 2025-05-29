using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<GetAllDepartmentsDto>>
    {
        readonly IDepartmentRepository _departmentRepository;
        private readonly ICacheService _cacheService;
        public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository, ICacheService cacheService)
        {
            _departmentRepository = departmentRepository;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<GetAllDepartmentsDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"Department_{request.pageNumber}_{request.pageSize}";

            ////getting from cache
            //var cachedDepartment = await _cacheService.GetAsync<IEnumerable<GetAllDepartmentsDto>>(cacheKey);
            //if (cachedDepartment !=null)
            //{
            //    return cachedDepartment;
            //}

            ////not in cache then fetching from repo 
            //var Departments = await _departmentRepository.GetAllDepartments(request.pageNumber, request.pageSize);

            ////store in cache 
            //await _cacheService.SetAsync(cacheKey, Departments, TimeSpan.FromMinutes(1));

            //return Departments;

            return await _departmentRepository.GetAllDepartments(request.pageNumber, request.pageSize);
        }
    }
}
