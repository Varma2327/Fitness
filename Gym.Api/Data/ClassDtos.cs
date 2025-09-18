namespace Gym.Api.DTOs
{
    public record ClassItemDto(
        int Id,
        string Title,
        string Description,
        int Capacity,
        DateTime StartUtc,
        DateTime EndUtc,
        int LocationId,
        string LocationName,
        bool Booked,
        int SpotsLeft
    );
}
