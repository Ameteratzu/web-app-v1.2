using DGPCE.Sigemad.Domain.Constracts;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;


namespace DGPCE.Sigemad.Infrastructure.Services;

public class CoordinateTransformationService : ICoordinateTransformationService
{
    public (double UTM_X, double UTM_Y, int Huso) ConvertToUTM(double latitude, double longitude)
    {
        // Determinar el huso UTM basándonos en la longitud
        int huso = (int)Math.Floor((longitude + 180) / 6) + 1;

        // Crear el sistema de coordenadas UTM
        var wgs84 = GeographicCoordinateSystem.WGS84;
        var utm = ProjectedCoordinateSystem.WGS84_UTM(huso, latitude >= 0);

        // Crear la transformación
        var transformation = new CoordinateTransformationFactory().CreateFromCoordinateSystems(wgs84, utm);

        // Realizar la transformación
        double[] utmCoordinates = transformation.MathTransform.Transform(new double[] { longitude, latitude });

        return (utmCoordinates[0], utmCoordinates[1], huso);
    }
}