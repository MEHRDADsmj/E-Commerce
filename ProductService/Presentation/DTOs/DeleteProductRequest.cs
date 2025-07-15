using System.ComponentModel.DataAnnotations;

namespace ProductService.Presentation.DTOs;

public class DeleteProductRequestDto
{
    [Required] public Guid Id { get; init; }

    public DeleteProductRequestDto(Guid id)
    {
        Id = id;
    }
}