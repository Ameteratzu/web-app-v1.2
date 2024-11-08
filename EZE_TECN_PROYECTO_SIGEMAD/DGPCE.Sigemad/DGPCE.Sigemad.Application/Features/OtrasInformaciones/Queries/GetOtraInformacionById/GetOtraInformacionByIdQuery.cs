using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
public class GetOtraInformacionByIdQuery : IRequest<List<OtraInformacionVm>> 
{
    public int Id { get; set; }

    public GetOtraInformacionByIdQuery(int id)
    {
        Id = id;
    }
}
