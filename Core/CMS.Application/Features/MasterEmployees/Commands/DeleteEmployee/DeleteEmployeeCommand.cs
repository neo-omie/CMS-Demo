using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEmployees.Commands.DeleteEmployee
{
    public record DeleteEmployeeCommand(int ValueId):IRequest<bool>;
 }
