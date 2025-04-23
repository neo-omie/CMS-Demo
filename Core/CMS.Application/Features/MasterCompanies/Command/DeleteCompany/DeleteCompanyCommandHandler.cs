using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.MasterCompanies.Command.DeleteCompany
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IMasterCompanyRepository _comprepo;

        public DeleteCompanyCommandHandler(IMasterCompanyRepository comprepo)
        {
            _comprepo = comprepo;
        }
        public Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            return _comprepo.DeleteCompanyAsync(request.id);
        }
    }
}
