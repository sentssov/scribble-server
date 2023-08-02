using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Scribble.Content.Application.Groups.Commands.CreateGroup;
using Scribble.Content.Application.Groups.Commands.UpdateGroup;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.UnitTests.Groups.Commands;

public class UpdateGroupCommandHandlerTests
{
    private readonly Mock<IEntityRepository> _repository;

    public UpdateGroupCommandHandlerTests() => 
        _repository = new Mock<IEntityRepository>();

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCommandIsValid()
    {
        var command = new UpdateGroupCommand(GroupId.New(),"new-name", "new-short-name", "new-description");

        _repository.Setup(
                x => x.UniqueByAsync<Group, GroupId>(
                    It.IsAny<Expression<Func<Group, bool>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _repository.Setup(
                x => x.GetAsync<Group, GroupId>(
                    It.IsAny<GroupId>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<Group>());

        var handler = new UpdateGroupCommandHandler(_repository.Object);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }
}