namespace CodeWF.Markdown.Themes;

/// <summary>
/// Describes one typography resource set shipped by CodeWF.Markdown.Themes.
/// </summary>
public sealed record MarkdownTypographyTheme(string Name, string Key);

public static class MarkdownTypographyThemes
{
    public const string OrangeHeart = "OrangeHeart";
    public const string InkBlack = "InkBlack";
    public const string ColorfulPurple = "ColorfulPurple";
    public const string TenderGreen = "TenderGreen";
    public const string Verdant = "Verdant";
    public const string RedScarlet = "RedScarlet";
    public const string BlueGlow = "BlueGlow";
    public const string TechnologyBlue = "TechnologyBlue";
    public const string LanQing = "LanQing";
    public const string Yamabuki = "Yamabuki";
    public const string FrontendPeak = "FrontendPeak";
    public const string GeekBlack = "GeekBlack";
    public const string Simple = "Simple";
    public const string RosePurple = "RosePurple";
    public const string CuteGreen = "CuteGreen";
    public const string FullStackBlue = "FullStackBlue";

    public static IReadOnlyList<MarkdownTypographyTheme> All { get; } =
    [
        new("橙心", OrangeHeart),
        new("墨黑", InkBlack),
        new("彩紫", ColorfulPurple),
        new("嫩青", TenderGreen),
        new("绿意", Verdant),
        new("红绯", RedScarlet),
        new("蓝萤", BlueGlow),
        new("科技蓝", TechnologyBlue),
        new("兰青", LanQing),
        new("山吹", Yamabuki),
        new("前端之峰", FrontendPeak),
        new("极客黑", GeekBlack),
        new("简洁", Simple),
        new("蔷薇紫", RosePurple),
        new("萌绿", CuteGreen),
        new("全栈蓝", FullStackBlue),
    ];
}
