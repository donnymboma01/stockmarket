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

    public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDto createCommentRequestDto, int stockId)
    {
        return new Comment
        {
            Title = createCommentRequestDto.Title,
            Content = createCommentRequestDto.Content,
            // StockId = createCommentRequestDto.StockId
            StockId = stockId
        };
    }

    public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentRequestDto)
    {
        return new Comment
        {
            Title = commentRequestDto.Title,
            Content = commentRequestDto.Content,
        };
    }
}