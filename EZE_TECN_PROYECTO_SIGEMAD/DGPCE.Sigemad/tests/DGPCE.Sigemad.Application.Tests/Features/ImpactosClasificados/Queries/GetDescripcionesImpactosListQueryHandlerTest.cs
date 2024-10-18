using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
using DGPCE.Sigemad.Application.Mappings;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.ImpactosClasificados.Queries;
public class GetDescripcionesImpactosListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetDescripcionImpactosListQueryHandler _handler;
    private readonly IMapper _mapper;

    public GetDescripcionesImpactosListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();        

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _handler = new GetDescripcionImpactosListQueryHandler(_unitOfWorkMock.Object, _mapper);
    }


    [Fact]
    public async Task Handle_ValidImpactosClasificados_ShouldReturnListOfDescripcionesImpactos()
    {
        // Arrange
        var request = new GetDescripcionImpactosListQuery();

        var impactos = new List<ImpactoClasificado>
        {
            new ImpactoClasificado { Id = 1, Descripcion = "Fallecidos" },
            new ImpactoClasificado { Id = 2, Descripcion = "Desaparecidos" },
            new ImpactoClasificado { Id = 3, Descripcion = "Personas sin hogar" }
        };

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetAllAsync()).ReturnsAsync(impactos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Fallecidos");

        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Desaparecidos");

        result[2].Id.Should().Be(3);
        result[2].Descripcion.Should().Be("Personas sin hogar");
    }

    [Fact]
    public async Task Handle_NoImpactosClasificados_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetDescripcionImpactosListQuery();
        var impactos = new List<ImpactoClasificado>();

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetAllAsync()).ReturnsAsync(impactos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_NullImpactosClasificados_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetDescripcionImpactosListQuery();

        _unitOfWorkMock.Setup(u => u.Repository<ImpactoClasificado>().GetAllAsync()).ReturnsAsync((List<ImpactoClasificado>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
