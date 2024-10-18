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
        result.ShouldHaveValidationErrorFor(c => c.IdIncendio)
            .WithErrorMessage("IdIncendio no puede estar en blanco");
    }



    [Fact]
    public void Validator_WithEmptyIdProvinciaAfectada_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEvolucionCommand
        {
            IdIncendio = 1,
            FechaHoraEvolucion = DateTime.Parse("2024-10-09T14:05:59Z"),
            IdProvinciaAfectada = ,
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
        result.ShouldHaveValidationErrorFor(c => c.IdProvinciaAfectada)
            .WithErrorMessage("IdProvinciaAfectada no puede estar en blanco");
    }




    [Fact]
    public void Validator_WithEmptyIdMunicipioAfectado_ShouldHaveValidationError()
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
        result.ShouldHaveValidationErrorFor(c => c.IdMunicipioAfectado)
            .WithErrorMessage("IdProvinciaAfectada no puede estar en blanco");
    }




}
