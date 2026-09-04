using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace CodeWF.AvaloniaControlsDemo.Pages;

public partial class ColorDemo : UserControl, INotifyPropertyChanged
{
    private Color _solidColor = Color.Parse("#5B8FF9");
    private SolidColorBrush _solidColorBrush = new(Color.Parse("#5B8FF9"));
    private double _linearStartX;
    private double _linearStartY;
    private double _linearEndX = 0.4;
    private double _linearEndY = 0.6;
    private LinearGradientBrush _linearBrush = new();
    private double _radialCenterX = 0.5;
    private double _radialCenterY = 0.5;
    private double _radialOriginX = 0.5;
    private double _radialOriginY = 0.5;
    private double _radialRadiusX = 0.75;
    private double _radialRadiusY = 0.75;
    private RadialGradientBrush _radialBrush = new();

    public ColorDemo()
    {
        InitializeComponent();

        LinearGradientStops.CollectionChanged += LinearGradientStopsOnCollectionChanged;
        RadialGradientStops.CollectionChanged += RadialGradientStopsOnCollectionChanged;

        foreach (var stop in LinearGradientStops) stop.PropertyChanged += LinearStopOnPropertyChanged;
        foreach (var stop in RadialGradientStops) stop.PropertyChanged += RadialStopOnPropertyChanged;

        UpdateSolidCode();
        UpdateLinearBrush();
        UpdateRadialBrush();
    }

    public ObservableCollection<GradientStopEditor> LinearGradientStops { get; } =
    [
        new(Color.Parse("#77FFD8BF"), 0d),
        new(Color.Parse("#B3FF9E80"), 0.45d),
        new(Color.Parse("#00FFFFFF"), 1d)
    ];

    public ObservableCollection<GradientStopEditor> RadialGradientStops { get; } =
    [
        new(Color.Parse("#FF4F46E5"), 0d),
        new(Color.Parse("#FF22D3EE"), 0.58d),
        new(Color.Parse("#00111827"), 1d)
    ];

    public Color SolidColor
    {
        get => _solidColor;
        set
        {
            if (!SetField(ref _solidColor, value, nameof(SolidColor))) return;

            SolidColorBrush = new SolidColorBrush(value);
            OnPropertyChanged(nameof(SolidColorBrush));
            UpdateSolidCode();
        }
    }

    public SolidColorBrush SolidColorBrush
    {
        get => _solidColorBrush;
        private set => _solidColorBrush = value;
    }

    public string SolidCode { get; private set; } = string.Empty;

    public double LinearStartX
    {
        get => _linearStartX;
        set
        {
            if (!SetField(ref _linearStartX, ClampUnit(value), nameof(LinearStartX))) return;
            UpdateLinearBrush();
        }
    }

    public double LinearStartY
    {
        get => _linearStartY;
        set
        {
            if (!SetField(ref _linearStartY, ClampUnit(value), nameof(LinearStartY))) return;
            UpdateLinearBrush();
        }
    }

    public double LinearEndX
    {
        get => _linearEndX;
        set
        {
            if (!SetField(ref _linearEndX, ClampUnit(value), nameof(LinearEndX))) return;
            UpdateLinearBrush();
        }
    }

    public double LinearEndY
    {
        get => _linearEndY;
        set
        {
            if (!SetField(ref _linearEndY, ClampUnit(value), nameof(LinearEndY))) return;
            UpdateLinearBrush();
        }
    }

    public LinearGradientBrush LinearBrush
    {
        get => _linearBrush;
        private set
        {
            _linearBrush = value;
            OnPropertyChanged(nameof(LinearBrush));
        }
    }

    public string LinearCode { get; private set; } = string.Empty;

    public double RadialCenterX
    {
        get => _radialCenterX;
        set
        {
            if (!SetField(ref _radialCenterX, ClampUnit(value), nameof(RadialCenterX))) return;
            UpdateRadialBrush();
        }
    }

    public double RadialCenterY
    {
        get => _radialCenterY;
        set
        {
            if (!SetField(ref _radialCenterY, ClampUnit(value), nameof(RadialCenterY))) return;
            UpdateRadialBrush();
        }
    }

    public double RadialOriginX
    {
        get => _radialOriginX;
        set
        {
            if (!SetField(ref _radialOriginX, ClampUnit(value), nameof(RadialOriginX))) return;
            UpdateRadialBrush();
        }
    }

    public double RadialOriginY
    {
        get => _radialOriginY;
        set
        {
            if (!SetField(ref _radialOriginY, ClampUnit(value), nameof(RadialOriginY))) return;
            UpdateRadialBrush();
        }
    }

    public double RadialRadiusX
    {
        get => _radialRadiusX;
        set
        {
            if (!SetField(ref _radialRadiusX, ClampUnit(value), nameof(RadialRadiusX))) return;
            UpdateRadialBrush();
        }
    }

    public double RadialRadiusY
    {
        get => _radialRadiusY;
        set
        {
            if (!SetField(ref _radialRadiusY, ClampUnit(value), nameof(RadialRadiusY))) return;
            UpdateRadialBrush();
        }
    }

    public RadialGradientBrush RadialBrush
    {
        get => _radialBrush;
        private set
        {
            _radialBrush = value;
            OnPropertyChanged(nameof(RadialBrush));
        }
    }

    public string RadialCode { get; private set; } = string.Empty;

    public new event PropertyChangedEventHandler? PropertyChanged;

    private void AddLinearStopClick(object? sender, RoutedEventArgs e) => AddStop(LinearGradientStops);

    private void AddRadialStopClick(object? sender, RoutedEventArgs e) => AddStop(RadialGradientStops);

    private void RemoveLinearStopClick(object? sender, RoutedEventArgs e) => RemoveStop(LinearGradientStops, sender);

    private void RemoveRadialStopClick(object? sender, RoutedEventArgs e) => RemoveStop(RadialGradientStops, sender);

    private void LinearGradientStopsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateStopSubscriptions(e, LinearStopOnPropertyChanged);
        UpdateLinearBrush();
    }

    private void RadialGradientStopsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateStopSubscriptions(e, RadialStopOnPropertyChanged);
        UpdateRadialBrush();
    }

    private void LinearStopOnPropertyChanged(object? sender, PropertyChangedEventArgs e) => UpdateLinearBrush();

    private void RadialStopOnPropertyChanged(object? sender, PropertyChangedEventArgs e) => UpdateRadialBrush();

    private void UpdateLinearBrush()
    {
        LinearBrush = new LinearGradientBrush
        {
            StartPoint = ToRelativePoint(LinearStartX, LinearStartY),
            EndPoint = ToRelativePoint(LinearEndX, LinearEndY),
            GradientStops = CreateGradientStops(LinearGradientStops)
        };

        UpdateLinearCode();
    }

    private void UpdateRadialBrush()
    {
        RadialBrush = new RadialGradientBrush
        {
            Center = ToRelativePoint((double)RadialCenterX, (double)RadialCenterY),
            GradientOrigin = ToRelativePoint((double)RadialOriginX, (double)RadialOriginY),
            RadiusX = new RelativeScalar(RadialRadiusX, RelativeUnit.Relative),
            RadiusY = new RelativeScalar(RadialRadiusY, RelativeUnit.Relative),
            GradientStops = CreateGradientStops(RadialGradientStops)
        };

        UpdateRadialCode();
    }

    private void UpdateSolidCode()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<ColorPicker Color=\"{FormatColor(SolidColor)}\"");
        builder.AppendLine("             IsAlphaVisible=\"True\"");
        builder.AppendLine("             IsColorComponentsVisible=\"True\"");
        builder.AppendLine("             IsColorPaletteVisible=\"True\"");
        builder.AppendLine("             IsColorSpectrumVisible=\"True\"");
        builder.Append("             IsHexInputVisible=\"True\" />");
        SolidCode = builder.ToString();
        OnPropertyChanged(nameof(SolidCode));
    }

    private void UpdateLinearCode()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<LinearGradientBrush StartPoint=\"{FormatPoint(LinearStartX, LinearStartY)}\"");
        builder.AppendLine($"                     EndPoint=\"{FormatPoint(LinearEndX, LinearEndY)}\">");
        builder.AppendLine("    <GradientStops>");

        foreach (var stop in LinearGradientStops.OrderBy(stop => stop.Offset))
            builder.AppendLine($"        <GradientStop Color=\"{FormatColor(stop.Color)}\" Offset=\"{FormatUnit(stop.Offset)}\" />");

        builder.AppendLine("    </GradientStops>");
        builder.Append("</LinearGradientBrush>");
        LinearCode = builder.ToString();
        OnPropertyChanged(nameof(LinearCode));
    }

    private void UpdateRadialCode()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<RadialGradientBrush Center=\"{FormatPoint(RadialCenterX, RadialCenterY)}\"");
        builder.AppendLine($"                     GradientOrigin=\"{FormatPoint(RadialOriginX, RadialOriginY)}\"");
        builder.AppendLine($"                     RadiusX=\"{FormatPercentage(RadialRadiusX)}\"");
        builder.AppendLine($"                     RadiusY=\"{FormatPercentage(RadialRadiusY)}\">");
        builder.AppendLine("    <GradientStops>");

        foreach (var stop in RadialGradientStops.OrderBy(stop => stop.Offset))
            builder.AppendLine($"        <GradientStop Color=\"{FormatColor(stop.Color)}\" Offset=\"{FormatUnit(stop.Offset)}\" />");

        builder.AppendLine("    </GradientStops>");
        builder.Append("</RadialGradientBrush>");
        RadialCode = builder.ToString();
        OnPropertyChanged(nameof(RadialCode));
    }

    private static GradientStops CreateGradientStops(IEnumerable<GradientStopEditor> editors)
    {
        var gradientStops = new GradientStops();

        foreach (var editor in editors.OrderBy(stop => stop.Offset))
            gradientStops.Add(new GradientStop(editor.Color, editor.Offset));

        return gradientStops;
    }

    private static void AddStop(ObservableCollection<GradientStopEditor> stops)
    {
        var ordered = stops.OrderBy(stop => stop.Offset).ToList();
        if (ordered.Count < 2)
        {
            stops.Add(new GradientStopEditor(Colors.White, 0.5d));
            return;
        }

        var left = ordered[0];
        var right = ordered[1];
        var largestGap = right.Offset - left.Offset;

        for (var index = 1; index < ordered.Count - 1; index++)
        {
            var currentGap = ordered[index + 1].Offset - ordered[index].Offset;
            if (currentGap <= largestGap) continue;

            left = ordered[index];
            right = ordered[index + 1];
            largestGap = currentGap;
        }

        var offset = (left.Offset + right.Offset) / 2d;
        stops.Add(new GradientStopEditor(Interpolate(left.Color, right.Color), offset));
    }

    private static void RemoveStop(ObservableCollection<GradientStopEditor> stops, object? sender)
    {
        if (stops.Count <= 2 || sender is not Button { Tag: GradientStopEditor stop }) return;
        stops.Remove(stop);
    }

    private static Color Interpolate(Color first, Color second)
    {
        return Color.FromArgb(
            InterpolateByte(first.A, second.A),
            InterpolateByte(first.R, second.R),
            InterpolateByte(first.G, second.G),
            InterpolateByte(first.B, second.B));
    }

    private static byte InterpolateByte(byte first, byte second) => (byte)((first + second) / 2);

    private static void UpdateStopSubscriptions(
        NotifyCollectionChangedEventArgs e,
        PropertyChangedEventHandler handler)
    {
        if (e.OldItems is not null)
            foreach (GradientStopEditor stop in e.OldItems)
                stop.PropertyChanged -= handler;

        if (e.NewItems is not null)
            foreach (GradientStopEditor stop in e.NewItems)
                stop.PropertyChanged += handler;
    }

    private static RelativePoint ToRelativePoint(double x, double y) =>
        new(x, y, RelativeUnit.Relative);

    private static double ClampUnit(double value) => Math.Clamp(value, 0d, 1d);

    private static string FormatColor(Color color) =>
        $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";

    private static string FormatPoint(double x, double y) =>
        $"{FormatPercentage(x)}, {FormatPercentage(y)}";

    private static string FormatPercentage(double value) =>
        $"{FormatUnit(value * 100d)}%";

    private static string FormatUnit(double value) =>
        value.ToString("0.##", CultureInfo.InvariantCulture);

    private bool SetField<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public sealed class GradientStopEditor : INotifyPropertyChanged
{
    private Color _color;
    private double _offset;

    public GradientStopEditor(Color color, double offset)
    {
        _color = color;
        _offset = Math.Clamp(offset, 0d, 1d);
    }

    public Color Color
    {
        get => _color;
        set
        {
            if (_color == value) return;

            _color = value;
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(ColorBrush));
        }
    }

    public SolidColorBrush ColorBrush => new(Color);

    public double Offset
    {
        get => _offset;
        set
        {
            var next = Math.Clamp(value, 0d, 1d);
            if (_offset == next) return;

            _offset = next;
            OnPropertyChanged(nameof(Offset));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
