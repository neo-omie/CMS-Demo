using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.AddCompany
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, MasterCompany>
    {
        private readonly IMasterCompanyRepository _MasterRepository;

        public AddCompanyCommandHandler(IMasterCompanyRepository masterCompanyRepository, IMapper mapper)
        {
            _MasterRepository = masterCompanyRepository;
        }
        public Task<MasterCompany> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {

            return _MasterRepository.AddCompanyAsync(request.mastercomp);
        }
    }
}
