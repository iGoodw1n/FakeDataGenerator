using FakeUserDataGenerator.Data;
using Microsoft.AspNetCore.Mvc;

namespace FakeUserDataGenerator.Models;

public class ParametersForFake
{
    [FromQuery(Name = "seed")]
    public int Seed { get; set; }

    [FromQuery(Name = "region")]
    public string Locale { get; set; } = null!;

    [FromQuery(Name = "page")]
    public int Page { get; set; }

    [FromQuery(Name = "errors")]
    public float Errors { get; set; }

    public int Amount { get; set; } = Params.Pages;
}
