using System.Text.Json.Serialization;

namespace MockAPI.Domain.Entities;
public sealed class ProductData
{
	[JsonPropertyName("color")]
	public string? Color { get; set; }
	[JsonPropertyName("capacity")]
	public string? Capacity { get; set; }
	[JsonPropertyName("capacity GB")]
	public object? CapacityGB { get; set; }
	[JsonPropertyName("price")]
	public object Price { get; set; }
	[JsonPropertyName("generation")]
	public string? Generation { get; set; }
	[JsonPropertyName("year")]
	public int? Year { get; set; }
	[JsonPropertyName("CPU model")]
	public string? CpuModel { get; set; }
	[JsonPropertyName("Hard disk size")]
	public string? HardDiskSize { get; set; }
	[JsonPropertyName("Strap Colour")]
	public string? StrapColour { get; set; }
	[JsonPropertyName("Case Size")]
	public string? CaseSize { get; set; }
	[JsonPropertyName("Description")]
	public string? Description { get; set; }
	[JsonPropertyName("Screen size")]
	public double ScreenSize { get; set; }
}