namespace Inventory.Api.DTOs;

public record ProductResponse(
    Guid Id,
    string Code,
    string Description,
    int Balance,
    DateTime CreatedAt
);