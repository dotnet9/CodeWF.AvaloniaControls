using System;
using System.Collections.Generic;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Converters;

namespace CodeWF.AvaloniaControls.Markup;

public class IfBinding : MarkupExtension
{
    private readonly MultiBinding _binding = new();

    public object? ConditionContent;

    public int ConditionIndex = Constants.InvalidIndex;
    public object? FalseContent;
    public int FalseIndex = Constants.InvalidIndex;
    public object? TrueContent;
    public int TrueIndex = Constants.InvalidIndex;

    public IfBinding()
    {
    }

    public IfBinding(object? condition, object? trueValue, object? falseValue)
        : this()
    {
        Condition = condition;
        True = trueValue;
        False = falseValue;
    }

    public bool HasBindings => _binding.Bindings.Count > 0;

    public object? Condition
    {
        set => SetProperty(value, ref ConditionIndex, out ConditionContent);
    }

    public object? True
    {
        set => SetProperty(value, ref TrueIndex, out TrueContent);
    }

    public object? False
    {
        set => SetProperty(value, ref FalseIndex, out FalseContent);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return HasBindings ? ToBinding() : Evaluate(Array.Empty<object?>())!;
    }

    internal MultiBinding ToBinding()
    {
        _binding.Converter ??= new IfConverter(this);
        return _binding;
    }

    internal object? Evaluate(IList<object?> values)
    {
        var condition = ResolveValue(values, ConditionIndex, ConditionContent);
        if (ReferenceEquals(condition, BindingOperations.DoNothing)) return BindingOperations.DoNothing;

        return condition is true
            ? ResolveValue(values, TrueIndex, TrueContent)
            : ResolveValue(values, FalseIndex, FalseContent);
    }

    internal static object? ResolveValue(IList<object?> values, int index, object? content)
    {
        if (index == Constants.InvalidIndex) return content;

        return index < values.Count ? values[index] : BindingOperations.DoNothing;
    }

    private void SetProperty<T>(T value, ref int index, out T? storage)
    {
        if (index != Constants.InvalidIndex) throw new InvalidOperationException("Cannot reset the value.");

        if (value is BindingBase binding)
        {
            _binding.Bindings.Add(binding);
            index = _binding.Bindings.Count - 1;
            storage = default;
            return;
        }

        storage = value;
    }
}