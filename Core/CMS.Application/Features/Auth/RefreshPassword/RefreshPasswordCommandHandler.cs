using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.Auth.RefreshPassword
{
    public class RefreshPasswordCommandHandler : IRequestHandler<RefreshPasswordCommand, string>
    {
        readonly IAuthRepository _authRepository;
        public RefreshPasswordCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<string> Handle(RefreshPasswordCommand request, CancellationToken cancellationToken)
        {
            var changePswd = await _authRepository.RefreshPasswordAsync(request.RefreshPassword);
            return changePswd;
        }
    }
}
