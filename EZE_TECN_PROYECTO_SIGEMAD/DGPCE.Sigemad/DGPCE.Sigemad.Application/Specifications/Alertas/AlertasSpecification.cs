using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Alertas
{
    public class AlertasSpecification : BaseSpecification<Alerta>
    {
        public AlertasSpecification(AlertasSpecificationParams alertasParams)
           : base(
                   x =>
                    (string.IsNullOrEmpty(alertasParams.Search) || x.Descripcion!.Contains(alertasParams.Search)) &&
                    (!alertasParams.IdEstado.HasValue || x.EstadoAlertaId == alertasParams.IdEstado) &&
                    (!alertasParams.FechaAlerta.HasValue || x.FechaAlerta == alertasParams.FechaAlerta)
                 )
        {
            AddInclude(p => p.EstadoAlerta!);

            ApplyPaging(alertasParams.PageSize * (alertasParams.Page - 1), alertasParams.PageSize);

            if (!string.IsNullOrEmpty(alertasParams.Sort))
            {
                switch (alertasParams.Sort)
                {
                    case "descripcionAsc":
                        AddOrderBy(p => p.Descripcion!);
                        break;

                    case "descripcionDesc":
                        AddOrderByDescending(p => p.Descripcion!);
                        break;
                    case "fechaAlertaAsc":
                        AddOrderBy(p => p.FechaAlerta!);
                        break;

                    case "fechaAlertaDesc":
                        AddOrderByDescending(p => p.FechaAlerta!);                        
                        break;
                    case "estadoAsc":
                        AddOrderBy(p => p.EstadoAlertaId!);
                        break;

                    case "estadoDesc":
                        AddOrderByDescending(p => p.EstadoAlertaId!);
                        break;

                    default:
                        AddOrderBy(p => p.FechaCreacion!);
                        break;
                }
            }
        }
    }
}
