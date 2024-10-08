using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combina dos expresiones utilizando el operador AND lógico (AndAlso).
        /// Se asegura de que ambas expresiones compartan el mismo parámetro.
        /// </summary>
        /// <typeparam name="T">Tipo de la entidad sobre la que actúan las expresiones.</typeparam>
        /// <param name="expr1">Primera expresión.</param>
        /// <param name="expr2">Segunda expresión.</param>
        /// <returns>Expresión combinada utilizando AND lógico (AndAlso).</returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            // Obtenemos el parámetro de la primera expresión
            var parameter = Expression.Parameter(typeof(T));

            // Creamos un diccionario que mapea los parámetros de la segunda expresión a los de la primera
            var visitor = new ReplaceExpressionVisitor(new Dictionary<Expression, Expression>
            {
                { expr2.Parameters[0], parameter }
            });

            // Visitamos y reemplazamos los parámetros de la segunda expresión
            var secondBody = visitor.Visit(expr2.Body);

            // Combinamos ambas expresiones utilizando AndAlso
            var combinedBody = Expression.AndAlso(expr1.Body, secondBody);

            // Creamos y devolvemos la expresión combinada
            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }
    }

    /// <summary>
    /// Visitante de expresión que reemplaza los parámetros en una expresión.
    /// Utilizado para asegurar que ambas expresiones compartan el mismo parámetro al combinarlas.
    /// </summary>
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Dictionary<Expression, Expression> _replaceMap;

        public ReplaceExpressionVisitor(Dictionary<Expression, Expression> replaceMap)
        {
            _replaceMap = replaceMap ?? new Dictionary<Expression, Expression>();
        }

        /// <summary>
        /// Reemplaza los parámetros de la expresión si están presentes en el mapa de reemplazo.
        /// </summary>
        /// <param name="node">El parámetro a reemplazar.</param>
        /// <returns>El parámetro reemplazado, o el original si no se encuentra en el mapa.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            // Si el parámetro actual está en el diccionario, lo reemplaza
            if (_replaceMap.TryGetValue(node, out var replacement))
            {
                return replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
