using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesByIdIncendioList
{


    public class GetEvolucionesByIdIncendioListQueryHandler : IRequestHandler<GetEvolucionesByIdIncendioListQuery, IReadOnlyList<EvolucionVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEvolucionesByIdIncendioListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IReadOnlyList<EvolucionVm>> Handle(GetEvolucionesByIdIncendioListQuery request, CancellationToken cancellationToken)
        {
              var includes = new List<Expression<Func<Evolucion, object>>>
                {
                    e => e.Municipio,
                    e => e.Provincia,
                    e => e.Medio,
                    e => e.EntradaSalida,
                    e => e.Tecnico,
                    e => e.Incendio,
                    e => e.EntidadMenor.Distrito.Pais,
                    e => e.TipoRegistro,
                    e => e.EstadoIncendio,
                    e => e.EvolucionProcedenciaDestinos
                };

            IReadOnlyList<Evolucion> evoluciones = (await _unitOfWork.Repository<Evolucion>().GetAsync(
                e => e.IdIncendio == request.IdIncendio,
                null,
                includes))
                .OrderByDescending(e => e.FechaHoraEvolucion)
                .ToList()
                .AsReadOnly();

            foreach (var evolucion in evoluciones)
            {
                foreach (var epd in evolucion.EvolucionProcedenciaDestinos)
                {
                    epd.ProcedenciaDestino = await _unitOfWork.Repository<ProcedenciaDestino>().GetByIdAsync(epd.IdProcedenciaDestino);
                }
            }

            var evolucionesVm = _mapper.Map<IReadOnlyList<Evolucion>, IReadOnlyList<EvolucionVm>>(evoluciones);
            return evolucionesVm;

        }
    }
}
