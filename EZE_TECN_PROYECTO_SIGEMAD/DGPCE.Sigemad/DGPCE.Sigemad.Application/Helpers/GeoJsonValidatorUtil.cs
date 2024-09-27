using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Helpers;
public static class GeoJsonValidatorUtil
{
    public static bool IsGeometryInWgs84(Geometry geometry)
    {
        if (geometry == null)
        {
            return false;
        }

        foreach (var coordinate in geometry.Coordinates)
        {
            if (coordinate.X < -180 || coordinate.X > 180 || coordinate.Y < -90 || coordinate.Y > 90)
            {
                return false;
            }
        }

        return true;
    }

}