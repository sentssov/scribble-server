using FluentAssertions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Models.UnitTests.Entities;

public class GroupTests
{
    private readonly Group _group;

    public GroupTests()
    {
        var groupName = GroupName.Create("name", true).Value;
        var groupShortName = GroupShortName.Create("short-name", true).Value;
        var groupDescription = GroupDescription.Create("description").Value;
        
        _group = Group.Create(groupName, groupShortName, groupDescription, UserId.New()).Value;

        var postTitle = PostTitle.Create("title").Value;
        var postContent = PostContent.Create("content").Value;
        var postDescription = PostDescription.Create("description").Value;

        _group.CreatePost(postTitle, postContent, postDescription);
    }

    [Fact]
    public void CreatePost_Should_ReturnSuccessResult_And_AddNewPostToTheCollection()
    {
        var postCount = _group.Posts.Count;
        var postTitle = PostTitle.Create("title").Value;
        var postContent = PostContent.Create("content").Value;
        var postDescription = PostDescription.Create("description").Value;

        var result = _group.CreatePost(postTitle, postContent, postDescription);

        result.IsFailure.Should().BeFalse();
        _group.Posts.Count.Should().Be(postCount + 1);
    }

    [Fact]
    public void RemovePost_Should_ReturnSuccessResult_AndRemovePostFromTheCollection()
    {
        var postTitle = PostTitle.Create("title").Value;
        var postContent = PostContent.Create("content").Value;
        var postDescription = PostDescription.Create("description").Value;
        var postId = _group.CreatePost(postTitle, postContent, postDescription).Value.Id;
        var postCount = _group.Posts.Count;

        var result = _group.RemovePost(postId);

        result.IsFailure.Should().BeFalse();
        _group.Posts.Count.Should().Be(postCount - 1);
    }

    [Fact]
    public void RemovePost_Should_ReturnFailureResult_WhenPostWithSpecifiedIdWasNotFound()
    {
        var postId = PostId.New();

        var result = _group.RemovePost(postId);

        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public void Subscribe_Should_ReturnSuccessResult_WhenUserHasNotBeenSubscribed()
    {
        var subscriptionCount = _group.Subscriptions.Count;
        var userId = UserId.New();

        var result = _group.CreateSubscription(userId);

        result.IsFailure.Should().BeFalse();
        _group.Subscriptions.Count.Should().Be(subscriptionCount + 1);
    }
    
    [Fact]
    public void Subscribe_Should_ReturnFailureResult_WhenUserHasAlreadyBeenSubscribed()
    {
        var userId = UserId.New();
        _group.CreateSubscription(userId);
        var subscriptionCount = _group.Subscriptions.Count;

        var result = _group.CreateSubscription(userId);

        result.IsFailure.Should().BeTrue();
        _group.Subscriptions.Count.Should().Be(subscriptionCount);
    }
}