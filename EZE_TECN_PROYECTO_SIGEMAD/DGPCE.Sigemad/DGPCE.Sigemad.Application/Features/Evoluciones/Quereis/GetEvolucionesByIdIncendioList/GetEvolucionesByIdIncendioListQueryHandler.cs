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

            var includes = new List<Expression<Func<Evolucion, object>>>();
            includes.Add(e => e.Municipio);
            includes.Add(e => e.Provincia);
            includes.Add(e => e.Medio);
            includes.Add(e => e.ProcedenciaDestino);
            includes.Add(e => e.EntradaSalida);
            includes.Add(e => e.Tecnico);
            includes.Add(e => e.Incendio);
            includes.Add(e => e.EstadoEvolucion);

 
            IReadOnlyList<Evolucion> evoluciones = (await _unitOfWork.Repository<Evolucion>().GetAsync(e => e.IdIncendio == request.IdIncendio, null, includes))
                    .OrderByDescending(e => e.FechaHoraEvolucion)          
                    .ToList()
                    .AsReadOnly();
          
            var evolucionesVm = _mapper.Map<IReadOnlyList<Evolucion>, IReadOnlyList<EvolucionVm>>(evoluciones);

            return evolucionesVm;

        }
    }
}
