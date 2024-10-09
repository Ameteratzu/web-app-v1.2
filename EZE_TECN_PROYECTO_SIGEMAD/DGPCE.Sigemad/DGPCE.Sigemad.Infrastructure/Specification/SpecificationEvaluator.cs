using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Infrastructure.Specification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            if (spec.Criteria != null)
            {
                inputQuery = inputQuery.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                inputQuery = inputQuery.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                inputQuery = inputQuery.OrderBy(spec.OrderByDescending);
            }

            if (spec.IsPagingEnable)
            {
                inputQuery = inputQuery.Skip(spec.Skip.Value).Take(spec.Take.Value);
            }

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

            return inputQuery;
        }

    }
}
