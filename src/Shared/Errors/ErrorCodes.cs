namespace TikTokFeed.Contracts.Errors;

public static class ErrorCodes
{
    public static string ValidationError => "VALIDATION_ERROR";

    public static string Unauthorized => "UNAUTHORIZED";

    public static string Forbidden => "FORBIDDEN";

    public static string NotFound => "NOT_FOUND";

    public static string Conflict => "CONFLICT";

    public static string InternalError => "INTERNAL_ERROR";

    public static string LikeAlreadyExists => "LIKE_ALREADY_EXISTS";

    public static string AlreadyFollowing => "ALREADY_FOLLOWING";

    public static string CannotFollowSelf => "CANNOT_FOLLOW_SELF";

    public static string AlreadyFavourite => "ALREADY_FAVOURITE";

    public static string VideoNotApproved => "VIDEO_NOT_APPROVED";

    public static string ParentCommentNotFound => "PARENT_COMMENT_NOT_FOUND";

    public static string EmailOrUsernameTaken => "EMAIL_OR_USERNAME_TAKEN";

    public static string InvalidCredentials => "INVALID_CREDENTIALS";
}
