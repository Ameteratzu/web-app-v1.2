using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Mappings;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Tests.Features.Evoluciones;
public class CreateEvolucionCommandHandlerTests
{

    private readonly Mock<ILogger<CreateEvolucionCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<IGeometryValidator> _geometryValidatorMock;
    private readonly Mock<IEvolucionService> _evolucionServiceMock;
    private readonly Mock<ICoordinateTransformationService> _coordinateTransformationServiceMock;

    public CreateEvolucionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateEvolucionCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _geometryValidatorMock = new Mock<IGeometryValidator>();
        _evolucionServiceMock = new Mock<IEvolucionService>();
        _coordinateTransformationServiceMock = new Mock<ICoordinateTransformationService>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
    }



        [Fact]
        public async Task Handle_GivenValidCommand_ShouldReturnSuccess()
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
                GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                    new NetTopologySuite.Geometries.LinearRing(new[]
                    {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                    }))
                {
                    SRID = 4326
                }
            };

            var incendio = new Incendio { Id = 1 };
            var municipio = new Municipio { Id = 1001 };
            var provincia = new Provincia { Id = 1 };
            var medio = new Medio { Id = 2 };
            var entradaSalida = new EntradaSalida { Id = 2 };
            var tecnico = new ApplicationUser { Id = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2") };
            var estadoIncendio = new EstadoIncendio { Id = 2 };
            var tipoRegistro = new TipoRegistro { Id = 1 };
            var entidadMenor = new EntidadMenor { Id = 208 };

            _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
                    .ReturnsAsync(incendio);
            _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
                    .ReturnsAsync(municipio);
            _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
                .ReturnsAsync(provincia);
            _unitOfWorkMock.Setup(u => u.Repository<Medio>().GetByIdAsync(command.IdMedio))
                .ReturnsAsync(medio);
            _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
                .ReturnsAsync(entradaSalida);
            _unitOfWorkMock.Setup(u => u.Repository<ApplicationUser>().GetByIdAsync(command.IdTecnico))
                .ReturnsAsync(tecnico);
            _unitOfWorkMock.Setup(u => u.Repository<EstadoIncendio>().GetByIdAsync(command.IdEstadoIncendio))
                .ReturnsAsync(estadoIncendio);
            _unitOfWorkMock.Setup(u => u.Repository<TipoRegistro>().GetByIdAsync(command.IdTipoRegistro))
                .ReturnsAsync(tipoRegistro);

            _unitOfWorkMock.Setup(u => u.Repository<EntidadMenor>().GetByIdAsync(command.IdEntidadMenor))
                .ReturnsAsync(entidadMenor);

            _geometryValidatorMock.Setup(g => g.IsGeometryValidAndInEPSG4326(It.IsAny<Geometry>()))
                .Returns(true);

            _coordinateTransformationServiceMock.Setup(u => u.ConvertToUTM(It.IsAny<Geometry>()))
                .Returns((Geometry geometry) => (500000.0, 4649776.22482, 30));

            _evolucionServiceMock.Setup(e => e.CrearNuevaEvolucion(It.IsAny<CreateEvolucionCommand>()))
                .ReturnsAsync(new Evolucion { Id = 1 });
   
        var handler = new CreateEvolucionCommandHandler(
                _loggerMock.Object,
                _unitOfWorkMock.Object,
                _geometryValidatorMock.Object,
                _evolucionServiceMock.Object
                );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreateEvolucionResponse>();
            result.Id.Should().Be(1);

    }



    [Fact]
    public async Task Handle_GivenInvalidIncendioId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
            .ReturnsAsync((Incendio)null);


        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Incendio)}*");
    }

    [Fact]
    public async Task Handle_GivenInvalidProvinciaAfectadaId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
       .ReturnsAsync((Provincia)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Provincia)}*");
    }

    [Fact]
    public async Task Handle_GivenInvalidMunicipioAfectadoId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
       .ReturnsAsync((Municipio)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Municipio)}*");
    }


    [Fact]
    public async Task Handle_GivenInvalidEntradaSalidaId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };
        var municipio = new Municipio { Id = 1001 };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
              .ReturnsAsync(municipio);

        _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
       .ReturnsAsync((EntradaSalida)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(EntradaSalida)}*");
    }

    [Fact]
    public async Task Handle_GivenInvalidTipoRegistroId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };
        var municipio = new Municipio { Id = 1001 };
        var entradaSalida = new EntradaSalida { Id = 2 };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
              .ReturnsAsync(municipio);

        _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
            .ReturnsAsync(entradaSalida);

        _unitOfWorkMock.Setup(u => u.Repository<TipoRegistro>().GetByIdAsync(command.IdTipoRegistro))
       .ReturnsAsync((TipoRegistro)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(TipoRegistro)}*");
    }

    [Fact]
    public async Task Handle_GivenInvalidTipoMedioId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };
        var municipio = new Municipio { Id = 1001 };
        var entradaSalida = new EntradaSalida { Id = 2 };
        var tipoRegistro = new TipoRegistro { Id = 1 };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
              .ReturnsAsync(municipio);

        _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
            .ReturnsAsync(entradaSalida);

        _unitOfWorkMock.Setup(u => u.Repository<TipoRegistro>().GetByIdAsync(command.IdTipoRegistro))
           .ReturnsAsync(tipoRegistro);

        _unitOfWorkMock.Setup(u => u.Repository<Medio>().GetByIdAsync(command.IdMedio))
       .ReturnsAsync((Medio)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Medio)}*");
    }

    [Fact]
    public async Task Handle_GivenInvalidTipoTecnicoId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };
        var municipio = new Municipio { Id = 1001 };
        var entradaSalida = new EntradaSalida { Id = 2 };
        var tipoRegistro = new TipoRegistro { Id = 1 };
        var medio = new Medio { Id = 2 };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
              .ReturnsAsync(municipio);

        _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
            .ReturnsAsync(entradaSalida);

        _unitOfWorkMock.Setup(u => u.Repository<TipoRegistro>().GetByIdAsync(command.IdTipoRegistro))
           .ReturnsAsync(tipoRegistro);

        _unitOfWorkMock.Setup(u => u.Repository<Medio>().GetByIdAsync(command.IdMedio))
            .ReturnsAsync(medio);

        _unitOfWorkMock.Setup(u => u.Repository<ApplicationUser>().GetByIdAsync(command.IdTecnico))
       .ReturnsAsync((ApplicationUser)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(ApplicationUser)}*");
    }


    [Fact]
    public async Task Handle_GivenInvalidEntidadMenorId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };
        var municipio = new Municipio { Id = 1001 };
        var entradaSalida = new EntradaSalida { Id = 2 };
        var tipoRegistro = new TipoRegistro { Id = 1 };
        var medio = new Medio { Id = 2 };
        var tecnico = new ApplicationUser { Id = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2") };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
              .ReturnsAsync(municipio);

        _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
            .ReturnsAsync(entradaSalida);

        _unitOfWorkMock.Setup(u => u.Repository<TipoRegistro>().GetByIdAsync(command.IdTipoRegistro))
           .ReturnsAsync(tipoRegistro);

        _unitOfWorkMock.Setup(u => u.Repository<Medio>().GetByIdAsync(command.IdMedio))
            .ReturnsAsync(medio);

        _unitOfWorkMock.Setup(u => u.Repository<ApplicationUser>().GetByIdAsync(command.IdTecnico))
           .ReturnsAsync(tecnico);

        _unitOfWorkMock.Setup(u => u.Repository<EntidadMenor>().GetByIdAsync(command.IdEntidadMenor))
       .ReturnsAsync((EntidadMenor)null);

        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(EntidadMenor)}*");
    }

    [Fact]
    public async Task Handle_GivenInvalidEstadoIncendioId_ShouldThrowNotFoundException()
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
            GeoPosicionAreaAfectada = new NetTopologySuite.Geometries.Polygon(
                new NetTopologySuite.Geometries.LinearRing(new[]
                {
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75422),
                        new NetTopologySuite.Geometries.Coordinate(-104.99011, 39.75621),
                        new NetTopologySuite.Geometries.Coordinate(-104.99404, 39.75621)
                }))
            {
                SRID = 4326
            }
        };


        var incendio = new Incendio { Id = 1 };
        var provincia = new Provincia { Id = 1 };
        var municipio = new Municipio { Id = 1001 };
        var entradaSalida = new EntradaSalida { Id = 2 };
        var tipoRegistro = new TipoRegistro { Id = 1 };
        var medio = new Medio { Id = 2 };
        var tecnico = new ApplicationUser { Id = Guid.Parse("D3813C04-4EEE-4D37-84B7-49EC293F92D2") };
        var entidadMenor = new EntidadMenor { Id = 208 };

        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().GetByIdAsync(command.IdIncendio))
           .ReturnsAsync(incendio);

        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvinciaAfectada))
              .ReturnsAsync(provincia);

        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipioAfectado))
              .ReturnsAsync(municipio);

        _unitOfWorkMock.Setup(u => u.Repository<EntradaSalida>().GetByIdAsync(command.IdEntradaSalida))
            .ReturnsAsync(entradaSalida);

        _unitOfWorkMock.Setup(u => u.Repository<TipoRegistro>().GetByIdAsync(command.IdTipoRegistro))
           .ReturnsAsync(tipoRegistro);

        _unitOfWorkMock.Setup(u => u.Repository<Medio>().GetByIdAsync(command.IdMedio))
            .ReturnsAsync(medio);

        _unitOfWorkMock.Setup(u => u.Repository<ApplicationUser>().GetByIdAsync(command.IdTecnico))
           .ReturnsAsync(tecnico);

        _unitOfWorkMock.Setup(u => u.Repository<EntidadMenor>().GetByIdAsync(command.IdEntidadMenor))
          .ReturnsAsync(entidadMenor);

        _unitOfWorkMock.Setup(u => u.Repository<EstadoIncendio>().GetByIdAsync(command.IdEstadoIncendio))
        .ReturnsAsync((EstadoIncendio)null);


        var handler = new CreateEvolucionCommandHandler(
        _loggerMock.Object,
        _unitOfWorkMock.Object,
        _geometryValidatorMock.Object,
        _evolucionServiceMock.Object
        );

        // Act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(EstadoIncendio)}*");
    }
}

