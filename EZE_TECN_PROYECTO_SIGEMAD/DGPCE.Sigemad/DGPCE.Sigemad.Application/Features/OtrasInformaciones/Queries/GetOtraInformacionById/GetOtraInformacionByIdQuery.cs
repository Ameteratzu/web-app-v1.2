using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
public class GetOtraInformacionByIdQuery : IRequest<OtraInformacion> 
{
    public int Id { get; set; }

    public GetOtraInformacionByIdQuery(int id)
    {
        Id = id;
    }
}
