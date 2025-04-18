using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand:IRequest
    {
        public string Id { get; set; }
    }
}
