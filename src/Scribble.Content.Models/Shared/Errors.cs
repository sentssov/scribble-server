namespace Scribble.Content.Models.Shared;

public static class Errors
{
    public static class Category
    {
        public static readonly Error NotExistsById = new(
            $"{nameof(Entities.Category)}.{nameof(NotExistsById)}",
            "The category with specified identifier does not exists.");
        
        public static readonly Error GroupAlreadyIncluded = new(
            $"{nameof(Entities.Category)}.{nameof(Entities.Category.IncludeGroup)}", 
            "The group has already been included to this category.");

        public static readonly Error GroupHasNotBeenIncluded = new(
            $"{nameof(Entities.Category)}.{nameof(Entities.Category.ExcludeGroup)}", 
            "The group has not been included to this category.");
    }
    
    public static class Comment
    {
        public static readonly Error NotExistsById = new(
            $"{nameof(Entities.Comment)}.{nameof(NotExistsById)}",
            "The comment with specified identifier does not exists.");
    }
    
    public static class Group
    {
        public static readonly Error NotExistsById = new(
            $"{nameof(Entities.Group)}.{nameof(NotExistsById)}",
            "The group with specified identifier does not exists.");

        public static readonly Error NotExistsByShortName = new(
            $"{nameof(Entities.Group)}.{nameof(NotExistsByShortName)}",
            "The group with specified short name does not exists.");
        
        public static readonly Error PostNotExists = new(
            $"{nameof(Entities.Group)}.{nameof(PostNotExists)}",
            "The post with specified identifier does not exists in the group.");
        
        public static readonly Error UserAlreadySubscribed = new(
            $"{nameof(Entities.Group)}.{nameof(UserAlreadySubscribed)}",
            "The user with specified identifier has already been subscribed.");

        public static readonly Error UserHasNotBeenSubscribed = new(
            $"{nameof(Entities.Group)}.{nameof(UserHasNotBeenSubscribed)}",
            "The user with specified identifier has not been subscribed yet.");
    }
    
    public static class Post
    {
        public static readonly Error NotExistsById = new(
            $"{nameof(Entities.Post)}.{nameof(NotExistsById)}",
            "The post with specified identifier does not exists.");
        
        public static readonly Error TagAlreadyAttached = new(
            $"{nameof(Entities.Post)}.{nameof(TagAlreadyAttached)}",
            "The tag with specified identifier has already been attached to the post.");

        public static readonly Error TagHasNotBeenAttached = new(
            $"{nameof(Entities.Post)}.{nameof(TagHasNotBeenAttached)}",
            "The tag with specified identifier has not been attached to the post yet.");
        
        public static readonly Error UserAlreadyLiked = new(
            $"{nameof(Entities.Post)}.{nameof(UserAlreadyLiked)}",
            "The user with specified identifier has already liked the post.");
        
        public static readonly Error UserHasNotLikedYet = new(
            $"{nameof(Entities.Post)}.{nameof(UserHasNotLikedYet)}",
            "The user with specified identifier has not liked the post yet.");
        
        public static readonly Error CommentNotExists = new(
            $"{nameof(Entities.Post)}.{nameof(CommentNotExists)}",
            "The comment with specified identifier does not exists in the post.");
    }
    
    public static class Tag
    {
        public static readonly Error NotExistsById = new(
            $"{nameof(Entities.Tag)}.{nameof(NotExistsById)}",
            "The tag with specified identifier does not exists.");
    }
}