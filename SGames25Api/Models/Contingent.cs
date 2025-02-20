using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SGames25Api.Models;

[ModelMetadataType(typeof(MetaDatas.ContingentMetaData))]
public class Contingent
{
    public int ID { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

     // Navigation property
    public ICollection<Athlete>? Athletes { get; set; }

}
