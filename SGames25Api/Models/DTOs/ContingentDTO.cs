using System;
using Microsoft.AspNetCore.Mvc;

namespace SGames25Api.Models.DTOs;

[ModelMetadataType(typeof(MetaDatas.ContingentMetaData))]

public class ContingentDTO
{
    public int ID { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    // Navigation property
    public ICollection<AthleteDTO>? Athletes { get; set; }

}
