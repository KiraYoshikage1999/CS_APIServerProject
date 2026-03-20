using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public record CharacteristicsDTO(string? state, string? typeGas, [Range(0, double.MaxValue)] int milege, string? typeMilege,
        string? typeBody, string? Color, string? DriveType, string? Engine);
}