
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Municipios.Quereis.GetMunicipioByIdProvincia
{
    public class GetMunicipioByIdProvinciaQuery : IRequest<IReadOnlyList<MunicipioSinIdProvinciaVm>>
    {

       public int IdProvincia { get; set; }


        public GetMunicipioByIdProvinciaQuery(int id)
        {
            IdProvincia = id;
        }

    }

    
}
