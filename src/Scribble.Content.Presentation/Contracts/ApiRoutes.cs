namespace Scribble.Content.Presentation.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;
    
    public static class Categories
    {
        private const string CategoryBase = Base + "/categories";

        public const string GetCategoryById = CategoryBase + "/{categoryId:guid}";
        public const string GetCategories = CategoryBase;
        public const string CreateCategory = CategoryBase;
        public const string UpdateCategory = CategoryBase + "/{categoryId:guid}";
        public const string RemoveCategory = CategoryBase + "/{categoryId:guid}";
        public const string IncludeGroup = CategoryBase + "/{categoryId:guid}/groups";
        public const string ExcludeGroup = CategoryBase + "/{categoryId:guid}/groups";
        public const string GetGroups = CategoryBase + "/{categoryId:guid}/groups";
    }
    
    public static class Comments
    {
        private const string CommentBase = Base + "/comments";

        public const string GetCommentById = CommentBase + "/{commentId:guid}";
        public const string UpdateComment = CommentBase + "/{commentId:guid}";
        public const string RemoveComment = CommentBase + "/{commentId:guid}";
    }
    
    public static class Groups
    {
        private const string GroupBase = Base + "/groups";
        
        public const string GetGroupById = GroupBase + "/{groupId:guid}";
        public const string GetGroupByShortName = GroupBase + "/{shortName:maxlength(45)}";
        public const string GetGroups = GroupBase;
        public const string GetGroupsCount = GroupBase + "/count";
        public const string CreateGroup = GroupBase;
        public const string UpdateGroup = GroupBase + "/{groupId:guid}";
        public const string RemoveGroup = GroupBase + "/{groupId:guid}";
        public const string GetPosts = GroupBase + "/{groupId:guid}/posts";
        public const string CreatePost = GroupBase + "/{groupId:guid}/posts";
        public const string CreateSubscription = GroupBase + "/{groupId:guid}/subscriptions";
        public const string RemoveSubscription = GroupBase + "/{groupId:guid}/subscriptions";
    }
    
    public static class Posts
    {
        private const string PostBase = Base + "/posts";

        public const string GetPostById = PostBase + "/{postId:guid}";
        public const string UpdatePost = PostBase + "/{postId:guid}";
        public const string RemovePost = PostBase + "/{postId:guid}";
        public const string GetComments = PostBase + "/{postId:guid}/comments";
        public const string CreateComment = PostBase + "/{postId:guid}/comments";
        public const string CreateLike = PostBase + "/{postId:guid}/likes";
        public const string RemoveLike = PostBase + "/{postId:guid}/likes";
    }
    
    public static class Tags
    {
        private const string TagBase = Base + "/tags";

        public const string GetTagById = TagBase + "/{tagId:guid}";
        public const string GetTags = TagBase;
        public const string CreateTag = TagBase;
        public const string UpdateTag = TagBase + "/{tagId:guid}";
        public const string RemoveTag = TagBase + "/{tagId:guid}";
    }
}