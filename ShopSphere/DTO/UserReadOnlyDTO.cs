namespace ShopSphere.DTO
{
    public record UserReadOnlyDTO(
        int Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string RoleName
    )
    {
    }
}
