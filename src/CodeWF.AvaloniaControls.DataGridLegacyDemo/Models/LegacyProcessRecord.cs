namespace CodeWF.AvaloniaControls.DataGridLegacyDemo.Models;

public sealed class LegacyProcessRecord
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Line { get; init; }

    public required string Host { get; init; }

    public required string ProgramPath { get; init; }

    public required string WorkPath { get; init; }

    public required string Parameters { get; init; }

    public required string Description { get; init; }

    public required string Status { get; init; }
}
