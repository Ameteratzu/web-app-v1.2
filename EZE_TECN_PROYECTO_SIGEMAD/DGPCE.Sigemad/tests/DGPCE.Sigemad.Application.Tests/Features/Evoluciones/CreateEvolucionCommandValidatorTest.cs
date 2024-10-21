using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;
using FluentValidation.TestHelper;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Tests.Features.Evoluciones;
public class CreateEvolucionCommandValidatorTest
{

    private readonly CreateEvolucionCommandValidator _validator;

    public CreateEvolucionCommandValidatorTest()
    {
        _validator = new CreateEvolucionCommandValidator();
    }




    [Fact]
    public void Validator_WithValidRequest_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateEvolucionCommand
        {
            IdIncendio = 1,
            FechaHoraEvolucion = DateTime.Parse("2024-10-09T14:05:59Z"),
            IdProvinciaAfectada = 1,
            IdMunicipioAfectado = 1001,
            IdEntradaSalida = 2,
            IdTipoRegistro = 1,
            IdMedio = 2,
            IdTecnico = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2"),
            Resumen = true,
            IdEntidadMenor = 208,
            Observaciones = "Contenido de prueba",
            Prevision = "Contenido de prueba",
            IdEstadoIncendio = 2,
            SuperficieAfectadaHectarea = 50,
            FechaFinal = DateTime.Parse("2024-10-25T14:05:59Z"),
            EvolucionProcedenciaDestinos = new List<int> { 1, 2, 3 },
            GeoPosicionAreaAfectada = new Point(-2, 42) { SRID = 4326 }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public void Validator_WithEmptyIdIncendio_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEvolucionCommand
        {
            IdIncendio = 1,
            FechaHoraEvolucion = DateTime.Parse("2024-10-09T14:05:59Z"),
            IdProvinciaAfectada = 1,
            IdMunicipioAfectado = 1001,
            IdEntradaSalida = 2,
            IdTipoRegistro = 1,
            IdMedio = 2,
            IdTecnico = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2"),
            Resumen = true,
            IdEntidadMenor = 208,
            Observaciones = "Contenido de prueba",
            Prevision = "Contenido de prueba",
            IdEstadoIncendio = 2,
            SuperficieAfectadaHectarea = 50,
            FechaFinal = DateTime.Parse("2024-10-25T14:05:59Z"),
            EvolucionProcedenciaDestinos = new List<int> { 1, 2, 3 },
            GeoPosicionAreaAfectada = new Point(-2, 42) { SRID = 4326 }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }



    [Fact]
    public void Validator_WithEmptyFechaHoraEvolucion_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEvolucionCommand
        {
            IdProvinciaAfectada = 1,
            IdMunicipioAfectado = 1001,
            IdEntradaSalida = 2,
            IdTipoRegistro = 1,
            IdMedio = 2,
            IdTecnico = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2"),
            Resumen = true,
            IdEntidadMenor = 208,
            Observaciones = "Contenido de prueba",
            Prevision = "Contenido de prueba",
            IdEstadoIncendio = 2,
            SuperficieAfectadaHectarea = 50,
            FechaFinal = DateTime.Parse("2024-10-25T14:05:59Z"),
            EvolucionProcedenciaDestinos = new List<int> { 1, 2, 3 },
            GeoPosicionAreaAfectada = new Point(-2, 42) { SRID = 4326 }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FechaHoraEvolucion)
            .WithErrorMessage("FechaHoraEvolucion es obligatoria");
    }



    [Fact]
    public void Validator_WithEmptyIdTecnico_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEvolucionCommand
        {
            FechaHoraEvolucion = DateTime.Parse("2024-10-09T14:05:59Z"),
            IdProvinciaAfectada = 1,
            IdMunicipioAfectado = 1001,
            IdEntradaSalida = 2,
            IdTipoRegistro = 1,
            IdMedio = 2,
            Resumen = true,
            IdEntidadMenor = 208,
            Observaciones = "Contenido de prueba",
            Prevision = "Contenido de prueba",
            IdEstadoIncendio = 2,
            SuperficieAfectadaHectarea = 50,
            FechaFinal = DateTime.Parse("2024-10-25T14:05:59Z"),
            EvolucionProcedenciaDestinos = new List<int> { 1, 2, 3 },
            GeoPosicionAreaAfectada = new Point(-2, 42) { SRID = 4326 }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.IdTecnico)
            .WithErrorMessage("IdTecnico no puede estar en blanco");
    }



    [Fact]
    public void Validator_WithInvalidGeoPosicionAreaAfectada_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEvolucionCommand
        {
            IdIncendio = 1,
            FechaHoraEvolucion = DateTime.Parse("2024-10-09T14:05:59Z"),
            IdProvinciaAfectada = 1,
            IdMunicipioAfectado = 1001,
            IdEntradaSalida = 2,
            IdTipoRegistro = 1,
            IdMedio = 2,
            IdTecnico = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2"),
            Resumen = true,
            IdEntidadMenor = 208,
            Observaciones = "Contenido de prueba",
            Prevision = "Contenido de prueba",
            IdEstadoIncendio = 2,
            SuperficieAfectadaHectarea = 50,
            FechaFinal = DateTime.Parse("2024-10-25T14:05:59Z"),
            EvolucionProcedenciaDestinos = new List<int> { 1, 2, 3 },
            GeoPosicionAreaAfectada = new Point(3330, 1) { SRID = 3857 } // Wrong SRID (not WGS84)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.GeoPosicionAreaAfectada)
            .WithErrorMessage("La geometría no es válida, sistema de referencia no es Wgs84");
    }

    





}
