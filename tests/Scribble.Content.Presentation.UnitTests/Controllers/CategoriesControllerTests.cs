using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Scribble.Content.Application.Categories.Queries;
using Scribble.Content.Application.Categories.Queries.GetCategoryById;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Presentation.Controllers.V1.Entities;

namespace Scribble.Content.Presentation.UnitTests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly Mock<IMapper> _mapperMock;

    public CategoriesControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task GetCategoryById_Should_ReturnOkObjectResult()
    {
        _senderMock.Setup(
                x => x.Send(
                    It.IsAny<GetCategoryByIdQuery>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<CategoryResponse>());

        var controller = new CategoriesController(
            _senderMock.Object, 
            _mapperMock.Object);

        var result = await controller.GetCategoryById(Guid.NewGuid(), default);

        result.Should().BeOfType<OkObjectResult>();
    }
}