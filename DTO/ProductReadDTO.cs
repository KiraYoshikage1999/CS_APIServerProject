// PSEUDOCODE PLAN:
// - Create class ProductReadDTO with properties mapped from the original record.
// - Include attributes exactly as in the records.
// - Use auto-properties with get; set; and appropriate nullable annotations.
// - Add using directives and namespace to match existing project structure.
// - Keep validation attributes as provided (even if some are unconventional for numeric types).

using System;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class ProductReadDTO
    {
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string? Brand { get; set; }

        [Required, MaxLength(40)]
        public string? Model { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quanity { get; set; }

        public string? Currency { get; set; }

        [Required]
        public CharacteristicsDTO? Characteristics { get; set; }

        public string? ImageCode { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }
    }
}