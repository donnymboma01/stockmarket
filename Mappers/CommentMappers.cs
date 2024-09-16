using api.Dtos.Comments;
using api.Models;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            StockId = comment.StockId,
            Title = comment.Title,
            CreatedOn = comment.CreatedOn,
        };
    }
}