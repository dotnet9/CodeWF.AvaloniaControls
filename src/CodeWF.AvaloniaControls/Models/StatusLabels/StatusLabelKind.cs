using System.ComponentModel;

namespace CodeWF.AvaloniaControls.Models;

public enum StatusLabelKind
{
    [Description("调试")] Debug,
    [Description("消息")] Info,
    [Description("警告")] Warn,
    [Description("错误")] Error,
    [Description("严重错误")] Fatal
}