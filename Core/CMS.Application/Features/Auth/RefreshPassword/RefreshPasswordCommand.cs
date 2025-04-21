using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.DTOs;
using MediatR;

namespace CMS.Application.Features.Auth.RefreshPassword
{
    public record RefreshPasswordCommand(RefreshPasswordDto RefreshPassword) : IRequest<string>;
}
