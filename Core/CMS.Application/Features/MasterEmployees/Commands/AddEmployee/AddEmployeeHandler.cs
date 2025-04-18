using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.AddEmployee
{
    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, MasterEmployee>
    {
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;
      
        public AddEmployeeHandler(IMasterEmployeeRepository masterEmployeeRepository)
        {
            _masterEmployeeRepository = masterEmployeeRepository;
        }
        public Task<MasterEmployee> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {

        }
    }
}
