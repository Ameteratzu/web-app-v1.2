using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.Menus.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Text.RegularExpressions;
using System;

namespace DGPCE.Sigemad.Application.Features.Municipios.Quereis.GetMunicipioByIdProvincia
{
    public class GetMunicipioByIdProvinciaQueryHandler : IRequestHandler<GetMunicipioByIdProvinciaQuery, IReadOnlyList<MunicipioSinIdProvinciaVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMunicipioByIdProvinciaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<IReadOnlyList<MunicipioSinIdProvinciaVm>> Handle(GetMunicipioByIdProvinciaQuery request, CancellationToken cancellationToken)
        {

            IReadOnlyList<Municipio> municipiosListado = (await _unitOfWork.Repository<Municipio>().GetAsync(m => m.IdProvincia == request.IdProvincia))
             .OrderBy(m => m.Descripcion)
             .ToList()
             .AsReadOnly();

            var municipiosSinIdProvincia =  _mapper.Map<IReadOnlyList<Municipio>, IReadOnlyList<MunicipioSinIdProvinciaVm>>(municipiosListado);

            return municipiosSinIdProvincia;

        }
    }
}
