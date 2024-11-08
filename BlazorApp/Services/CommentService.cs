using BlazorApp.Models;
using BlazorApp.Repository;

namespace BlazorApp.Services;

public class CommentService
{
    ICommentRepository _commentRepository;
    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public int AddComment(ContactComment comment)
    {
        return _commentRepository.AddComment(ContactCommentMapper.MapToEntity(comment));
    }
}