namespace TestDataGridDemo.Models;

public class ProcessItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Enabled { get; set; }
    public int SourceNode { get; set; }
    public string? Host { get; set; }
    public string? ProgramPath { get; set; }
    public string? WorkPath { get; set; }
    public string? Params { get; set; }
    public bool AutoStart { get; set; }
    public string? PreProcess { get; set; }
    public string? PostProcess { get; set; }
    public string? Description { get; set; }
}