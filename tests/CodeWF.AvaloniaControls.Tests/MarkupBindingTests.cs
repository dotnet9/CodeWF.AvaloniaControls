using CodeWF.AvaloniaControls.Markup;

namespace CodeWF.AvaloniaControls.Tests;

public sealed class MarkupBindingTests
{
    [Fact]
    public void IfBindingReturnsTheTrueValue()
    {
        var binding = new IfBinding(true, "enabled", "disabled");

        var result = binding.ProvideValue(null!);

        Assert.Equal("enabled", result);
    }

    [Fact]
    public void IfBindingReturnsTheFalseValue()
    {
        var binding = new IfBinding(false, "enabled", "disabled");

        var result = binding.ProvideValue(null!);

        Assert.Equal("disabled", result);
    }

    [Fact]
    public void SwitchBindingMatchesCaseAndUsesDefault()
    {
        var binding = new SwitchBinding(
            "warning",
            [new SwitchCase { When = "warning", Then = 2 }],
            0);

        Assert.Equal(2, binding.ProvideValue(null!));

        var defaultBinding = new SwitchBinding(
            "unknown",
            [new SwitchCase { When = "warning", Then = 2 }],
            0);

        Assert.Equal(0, defaultBinding.ProvideValue(null!));
    }
}
