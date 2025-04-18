using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;

        public DeleteEmployeeCommandHandler(IMasterEmployeeRepository masterEmployeeRepository)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await _masterEmployeeRepository.DeleteEmployeeAsync(request.ValueId);
            return true;
        }
    }
}
