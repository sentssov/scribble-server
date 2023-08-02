using System.Linq.Expressions;
using Moq;
using FluentAssertions;
using Scribble.Content.Application.Groups.Commands.CreateGroup;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.UnitTests.Groups.Commands;

public class CreateGroupCommandHandlerTests
{
    private readonly Mock<IEntityRepository> _repository;

    public CreateGroupCommandHandlerTests() => 
        _repository = new Mock<IEntityRepository>();

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCommandIsValid()
    {
        var command = new CreateGroupCommand(UserId.New(), "name", "short-name", "description");

        _repository.Setup(
                x => x.SaveAsync<Group, GroupId>(
                    It.IsAny<Group>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroupId.New);
        
        var handler = new CreateGroupCommandHandler(_repository.Object);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenNameOrShortNameIsNotUnique()
    {
        var command = new CreateGroupCommand(UserId.New(), "name-is-not-unique", "short-name", "description");

        _repository.Setup(
                x => x.UniqueByAsync<Group, GroupId>(
                    It.IsAny<Expression<Func<Group, bool>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateGroupCommandHandler(_repository.Object);

        var result = await handler.Handle(command, default);

        result.IsFailure.Should().BeTrue();
    }
}