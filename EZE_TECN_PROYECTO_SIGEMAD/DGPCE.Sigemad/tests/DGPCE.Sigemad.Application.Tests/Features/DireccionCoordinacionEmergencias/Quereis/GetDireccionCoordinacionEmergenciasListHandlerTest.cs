using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasList;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.TipoDireccionEmergencias.Quereis.GetTipoDireccionEmergenciasList;
using DGPCE.Sigemad.Application.Features.TiposRegistros.Queries.GetTiposRegistrosList;
using DGPCE.Sigemad.Application.Mappings;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.DireccionCoordinacionEmergencias.Quereis;
public class GetDireccionCoordinacionEmergenciasListHandlerTest
{

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetDireccionCoordinacionEmergenciasHandler _handler;
    private readonly IMapper _mapper;

    public GetDireccionCoordinacionEmergenciasListHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _handler = new GetDireccionCoordinacionEmergenciasHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListTipoPlanes()
    {
        {
            // Arrange
            var request = new GetDireccionCoordinacionEmergenciasListQuery();

            var direccionCoordinacionEmergencias = new List<DireccionCoordinacionEmergencia>
        {
            new DireccionCoordinacionEmergencia
            {
                Id = 1,
                IdIncendio = 1,
                IdTipoDireccionEmergencia = 1,
                AutoridadQueDirige = "Autoridad 1",
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddHours(1),
                FechaInicioCECOPI = DateTime.Now,
                FechaFinCECOPI = DateTime.Now.AddHours(1),
                IdProvinciaCECOPI = 1,
                IdMunicipioCECOPI = 1,
                LugarCECOPI = "Lugar 1",
                GeoPosicionCECOPI = null,
                ObservacionesCECOPI = "Observaciones 1",
                FechaInicioPMA = DateTime.Now,
                FechaFinPMA = DateTime.Now.AddHours(1),
                IdProvinciaPMA = 2,
                IdMunicipioPMA = 2,
                LugarPMA = "Lugar 2",
                GeoPosicionPMA = null,
                ObservacionesPMA = "Observaciones 2",
                Incendio = new Incendio { Id = 1},
                TipoDireccionEmergencia = new TipoDireccionEmergencia { Id = 1, Descripcion = "Tipo 1" },
                ProvinciaCECOPI = new Provincia { Id = 1, Descripcion = "Provincia 1" },
                MunicipioCECOPI = new Municipio { Id = 1, Descripcion = "Municipio 1" },
                ProvinciaPMA = new Provincia { Id = 2, Descripcion = "Provincia 2" },
                MunicipioPMA = new Municipio { Id = 2, Descripcion = "Municipio 2" },
                ActivacionPlanEmergencia = new ActivacionPlanEmergencia { IdDireccionCoordinacionEmergencia = 1, NombrePlan = "Plan 1" }
            },
            new DireccionCoordinacionEmergencia
            {
                Id = 2,
                IdIncendio = 2,
                IdTipoDireccionEmergencia = 2,
                AutoridadQueDirige = "Autoridad 2",
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddHours(2),
                FechaInicioCECOPI = DateTime.Now,
                FechaFinCECOPI = DateTime.Now.AddHours(2),
                IdProvinciaCECOPI = 2,
                IdMunicipioCECOPI = 2,
                LugarCECOPI = "Lugar 2",
                GeoPosicionCECOPI = null,
                ObservacionesCECOPI = "Observaciones 2",
                FechaInicioPMA = DateTime.Now,
                FechaFinPMA = DateTime.Now.AddHours(2),
                IdProvinciaPMA = 3,
                IdMunicipioPMA = 3,
                LugarPMA = "Lugar 3",
                GeoPosicionPMA = null,
                ObservacionesPMA = "Observaciones 3",
                Incendio = new Incendio { Id = 2 },
                TipoDireccionEmergencia = new TipoDireccionEmergencia { Id = 2, Descripcion = "Tipo 2" },
                ProvinciaCECOPI = new Provincia { Id = 2, Descripcion = "Provincia 2" },
                MunicipioCECOPI = new Municipio { Id = 2, Descripcion = "Municipio 2" },
                ProvinciaPMA = new Provincia { Id = 3, Descripcion = "Provincia 3" },
                MunicipioPMA = new Municipio { Id = 3, Descripcion = "Municipio 3" },
                ActivacionPlanEmergencia = new ActivacionPlanEmergencia { IdDireccionCoordinacionEmergencia = 2, NombrePlan = "Plan 2" }
            }
        };

            var includes = new List<Expression<Func<DireccionCoordinacionEmergencia, object>>>
        {
            c => c.ActivacionPlanEmergencia
        };

            _unitOfWorkMock.Setup(m => m.Repository<DireccionCoordinacionEmergencia>().GetAsync(null, null, It.IsAny<List<Expression<Func<DireccionCoordinacionEmergencia, object>>>>()))
                           .ReturnsAsync(direccionCoordinacionEmergencias);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
        }

    }
}
