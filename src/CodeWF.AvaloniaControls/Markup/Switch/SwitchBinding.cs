using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Converters;

namespace CodeWF.AvaloniaControls.Markup;

public class SwitchBinding : MarkupExtension
{
    private readonly MultiBinding _binding = new();
    private readonly List<ResolvedSwitchCase> _cases = [];

    public int ValueIndex = Constants.InvalidIndex;
    public int DefaultIndex = Constants.InvalidIndex;

    public object? ValueContent;
    public object? DefaultContent;

    public SwitchBinding()
    {
    }

    public SwitchBinding(object? value, IEnumerable<SwitchCase>? cases, object? defaultValue)
        : this()
    {
        Value = value;
        SetCases(cases);
        Default = defaultValue;
    }

    public bool HasBindings => _binding.Bindings.Count > 0;

    public object? Value
    {
        set => SetProperty(value, ref ValueIndex, out ValueContent);
    }

    public object? Default
    {
        set => SetProperty(value, ref DefaultIndex, out DefaultContent);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return HasBindings ? ToBinding() : Evaluate(Array.Empty<object?>(), CultureInfo.CurrentCulture)!;
    }

    internal MultiBinding ToBinding()
    {
        _binding.Converter ??= new SwitchConverter(this);
        return _binding;
    }

    internal object? Evaluate(IList<object?> values, CultureInfo culture)
    {
        var value = IfBinding.ResolveValue(values, ValueIndex, ValueContent);
        if (ReferenceEquals(value, BindingOperations.DoNothing))
        {
            return BindingOperations.DoNothing;
        }

        foreach (var item in _cases)
        {
            if (ValuesMatch(value, item.When, culture))
            {
                return IfBinding.ResolveValue(values, item.ThenIndex, item.ThenContent);
            }
        }

        return IfBinding.ResolveValue(values, DefaultIndex, DefaultContent);
    }

    private void SetCases(IEnumerable<SwitchCase>? cases)
    {
        if (cases == null)
        {
            return;
        }

        foreach (var item in cases)
        {
            var thenIndex = Constants.InvalidIndex;
            SetProperty(item.Then, ref thenIndex, out var thenContent);
            _cases.Add(new ResolvedSwitchCase(item.When, thenIndex, thenContent));
        }
    }

    private void SetProperty<T>(T value, ref int index, out T? storage)
    {
        if (index != Constants.InvalidIndex)
        {
            throw new InvalidOperationException("Cannot reset the value.");
        }

        if (value is BindingBase binding)
        {
            _binding.Bindings.Add(binding);
            index = _binding.Bindings.Count - 1;
            storage = default;
            return;
        }

        storage = value;
    }

    private static bool ValuesMatch(object? value, object? expected, CultureInfo culture)
    {
        if (Equals(value, expected))
        {
            return true;
        }

        if (value == null || expected == null)
        {
            return false;
        }

        if (value is Enum && expected is string expectedText)
        {
            return string.Equals(value.ToString(), expectedText, StringComparison.OrdinalIgnoreCase);
        }

        if (expected is Enum && value is string valueText)
        {
            return string.Equals(valueText, expected.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        var valueType = value.GetType();
        var expectedType = expected.GetType();
        if (valueType == expectedType)
        {
            return false;
        }

        try
        {
            if (value is IConvertible && expected is IConvertible)
            {
                return Equals(value, Convert.ChangeType(expected, valueType, culture));
            }
        }
        catch (InvalidCastException)
        {
        }
        catch (FormatException)
        {
        }
        catch (OverflowException)
        {
        }

        return false;
    }

    private sealed record ResolvedSwitchCase(object? When, int ThenIndex, object? ThenContent);
}
