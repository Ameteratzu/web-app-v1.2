using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Provincias.Quereis.GetProvinciasByIdCCAAList
{
    public class GetProvinciasByIdCCAAListQuery : IRequest<IReadOnlyList<ProvinciaSinMunicipiosVm>>
    {
        public int IdCcaa { get; set; }


        public GetProvinciasByIdCCAAListQuery(int id)
        {
            IdCcaa = id;
        }
    }
}

