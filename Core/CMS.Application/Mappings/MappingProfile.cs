using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Features.ContractTypeMaster;
using CMS.Application.Features.Document;
using CMS.Application.Features.MasterCompanies;
using CMS.Application.Features.MasterEmployees.EmployeeDtos;
using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;

namespace CMS.Application.Mappings
{
    public class MappingProfile:Profile
    {
         public MappingProfile()
        {
            CreateMap<AddEmployeeDto, MasterEmployee>().ReverseMap();
            CreateMap<GetAllEmployeeDto, MasterEmployee>().ReverseMap();
            CreateMap<UpdateEmployeeDto, MasterEmployee>().ReverseMap();
            CreateMap<GetEmployeesByDepartmentIdAndEmpDetailsDto, MasterEmployee>().ReverseMap();
            CreateMap<MasterDocument,DocumentDTO>().ReverseMap();
            CreateMap<AddApostilleDto, MasterApostille>().ReverseMap();
            CreateMap<MasterApostille, GetAllApostilleDto>().ReverseMap();
            CreateMap<MasterApostille, UpdateApostilleDto>().ReverseMap();
            CreateMap<MasterCompany, GetMastersDTO>().ReverseMap();
            CreateMap<ContractTypeMasters, GetContractDTO>().ReverseMap();
        }
    }
}
