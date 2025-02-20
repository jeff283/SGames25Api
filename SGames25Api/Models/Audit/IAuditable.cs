using System;

namespace SGames25Api.Models.Audit;

public interface IAuditable
{
    string? CreatedBy { get; set; }
    DateTime? CreatedOn { get; set; }
    string? UpdatedBy { get; set; }
    DateTime? UpdatedOn { get; set; }
}
