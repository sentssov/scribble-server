﻿namespace Scribble.Content.Presentation.Contracts.Posts.Requests;

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public Guid BlogId { get; set; }
}