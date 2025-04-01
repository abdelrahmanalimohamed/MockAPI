namespace MockAPI.Domain.Entities;
public sealed class ProductData
{
	public string? Color { get; set; }
	public string? Capacity { get; set; }
	public object Price { get; set; }
	public string? Generation { get; set; }
	public int? Year { get; set; }
	public string? CpuModel { get; set; }
	public string? HardDiskSize { get; set; }
	public string? StrapColour { get; set; }
	public string? CaseSize { get; set; }
	public string? Description { get; set; }
	public double ScreenSize { get; set; }
}