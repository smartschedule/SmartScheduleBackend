namespace SmartSchedule.Application.DAL.Interfaces
{
    using SmartSchedule.Application.DTO.Authentication;

    public interface IJwtService
    {
        JwtTokenModel GenerateJwtToken(string email, int id, bool isAdmin);
        bool ValidateStringToken(string token);
        int GetUserIdFromToken(string token);
    }
}
