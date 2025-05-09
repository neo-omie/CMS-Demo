using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.UpdateCompany
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, MasterCompany>
    {
        private readonly IMasterCompanyRepository _comprepo;

        public UpdateCompanyCommandHandler(IMasterCompanyRepository comprepo)
        {
            _comprepo = comprepo;
        }
        public async Task<MasterCompany> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company =await _comprepo.GetCompanyByIdAsync(request.id);
            if (company==null)
            {
                throw new Exception($"Company Not Found");
            }
            return await _comprepo.UpdateCompanyAsync(request.id, request.comp);
        }
    }
}
