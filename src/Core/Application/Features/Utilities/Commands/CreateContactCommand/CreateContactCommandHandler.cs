using Application.Common.Interfaces;
using Application.Common.Wrappers;
using AutoMapper;
using Domain.Entities.Common;
using MediatR;

namespace Application.Features.Utilities.Commands.CreateContactCommand
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateContactCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<string>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var newContact = _mapper.Map<Contact>(request);

            var customer = await _unitOfWork.Repository<Contact>().AddAsync(newContact);

            await _unitOfWork.Save(cancellationToken);

            return new Response<string>(true, $"¡Te avisaremos pronto!");
        }
    }
}
