using Application.Common.Exceptions;
using System.Net;
using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using AutoMapper;

namespace Application.Features.Authenticate.Commands.RegisterCommand
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAccountService accountService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var user = await _accountService.RegisterAsync(request);

            if (user == null)
                throw new ApiException("No se pudo registrar al usuario.");

            await _unitOfWork.Save(cancellationToken);

            return user;

        }
    }
}