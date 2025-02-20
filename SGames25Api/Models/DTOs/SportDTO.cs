using System;
using Microsoft.AspNetCore.Mvc;
using SGames25Api.Models.Audit;

namespace SGames25Api.Models.DTOs;

[ModelMetadataType(typeof(MetaDatas.SportMetaData))]
public class SportDTO
{
    public int ID { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

     public byte[]? RowVersion { get; set; }

    // Navigation property
    public ICollection<AthleteDTO>? Athletes { get; set; }
}
