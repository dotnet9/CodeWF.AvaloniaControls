using System;
using System.Threading;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Styling;

namespace CodeWF.AvaloniaControls.Animations;

/// <summary>
/// 可等待的动效：把一段 Avalonia <see cref="Animation"/> 跑完并给出完成通知，
/// 带超时兜底（动画被取消、宿主切换时仍会放行等待方）。
/// <para>
/// Avalonia 12 的 <c>Transition</c> 已无公开的「开始/结束」可覆写点
/// （<c>DoTransition</c>/<c>Apply</c> 为 internal），因此这里基于 <see cref="Animation"/>
/// 提供同等的串行动效编排能力，用于「展开 → 再执行下一步」这类流程。
/// </para>
/// </summary>
public static class AwaitableAnimation
{
    private static readonly TimeSpan FallbackExtra = TimeSpan.FromMilliseconds(60);

    /// <summary>
    /// 运行动画并等待其结束；超出动画 Duration + 兜底余量后即使动画未回调也会返回，
    /// 保证调用方不被卡住。
    /// </summary>
    public static async Task RunAsync(
        Animatable target,
        Animation animation,
        CancellationToken cancellationToken = default)
    {
        var run = animation.RunAsync(target, cancellationToken);
        var timeout = animation.Duration + FallbackExtra;
        var completed = await Task.WhenAny(run, Task.Delay(timeout, cancellationToken)).ConfigureAwait(true);
        if (!ReferenceEquals(completed, run))
        {
            return;
        }

        await run.ConfigureAwait(true);
    }

    /// <summary>
    /// 构造「从 0 到 1」的单进度动画，供进度驱动控件（如
    /// <see cref="Controls.ContentExpansionAnimator"/>）复用：尺寸与透明度由同一个进度插值。
    /// </summary>
    public static Animation CreateProgressAnimation(
        TimeSpan duration,
        AvaloniaProperty<double> progressProperty,
        Easing? easing = null)
    {
        return new Animation
        {
            Duration = duration,
            Easing = easing ?? new CubicEaseInOut(),
            FillMode = FillMode.Forward,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(1d),
                    Setters = { new Setter(progressProperty, 1d) }
                }
            }
        };
    }
}
