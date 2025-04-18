using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;

namespace CMS.Application.Mappings
{
    public class MappingProfile:Profile
    {
         public MappingProfile()
        {
            CreateMap<AddEmployeeDto, MasterEmployee>().ReverseMap();
            CreateMap<GetAllEmployeeDto, MasterEmployee>().ReverseMap();
            CreateMap<UpdateEmployeeDto, MasterEmployee>().ReverseMap();
        }
    }
}
