using System;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;

namespace CodeWF.AvaloniaControls.Markup;

public class SwitchExtension : MarkupExtension
{
    public SwitchExtension()
    {
    }

    public SwitchExtension(object? value)
    {
        Value = value;
    }

    public object? Value { get; set; }

    [Content]
    public SwitchCases? Cases { get; set; } = [];

    public object? Default { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new SwitchBinding(Value, Cases, Default).ProvideValue(serviceProvider);
    }
}
