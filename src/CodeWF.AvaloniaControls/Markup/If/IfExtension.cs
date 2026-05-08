using System;
using Avalonia.Markup.Xaml;

namespace CodeWF.AvaloniaControls.Markup;

public class IfExtension : MarkupExtension
{
    public IfExtension()
    {
    }

    public IfExtension(object? condition, object? trueValue, object? falseValue)
    {
        Condition = condition;
        True = trueValue;
        False = falseValue;
    }

    public object? Condition { get; set; }

    public object? True { get; set; }

    public object? False { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new IfBinding(Condition, True, False).ProvideValue(serviceProvider);
    }
}
