using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ShiftSummaryDto {
    public DateOnly Date { get; set; }
    public string ShiftTypeName { get; set; } = null!;
    public List<User> Employees { get; set; } = [];
}