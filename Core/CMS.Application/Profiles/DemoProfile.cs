using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Features.Document;
using CMS.Domain.Entities;

namespace CMS.Application.Profiles
{
    public class DemoProfile : Profile
    {
        public DemoProfile()
        {
            CreateMap<DocumentDTO, MasterDocument>().ReverseMap();
        }
    }
}
