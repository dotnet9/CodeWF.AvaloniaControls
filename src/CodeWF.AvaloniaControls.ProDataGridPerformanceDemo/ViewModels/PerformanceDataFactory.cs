using System;
using System.Collections.Generic;
using CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.Models;

namespace CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.ViewModels;

internal static class PerformanceDataFactory
{
    public static IReadOnlyList<ProcessItem> CreateRows(int count, int seed, string scenarioName)
    {
        var random = new Random(seed);
        var items = new List<ProcessItem>(count);

        for (var i = 0; i < count; i++)
        {
            var sourceNode = random.Next(1, 16);
            var lineNo = seed % 8 + 1;
            var stationNo = i % 24 + 1;
            var isEnabled = (i + seed) % 3 != 0;
            var autoStart = (i + seed) % 5 == 0;

            items.Add(new ProcessItem
            {
                Id = i + 1,
                Name = $"{scenarioName}-工位-{stationNo:00}",
                Enabled = isEnabled,
                SourceNode = sourceNode,
                Host = $"10.{seed % 20}.{sourceNode}.{stationNo}",
                ProgramPath = $@"D:\Apps\Line{lineNo}\Worker{stationNo:00}\run.exe",
                WorkPath = $@"D:\Apps\Line{lineNo}\Worker{stationNo:00}",
                Params = autoStart ? "--boot --safe" : "--watch --batch",
                AutoStart = autoStart,
                PreProcess = isEnabled ? "校验缓存、准备上下文" : "等待人工确认",
                PostProcess = autoStart ? "上传日志并清理临时文件" : "归档结果并发送通知",
                Description = $"用于模拟 {scenarioName} 的大数据量页签切换场景"
            });
        }

        return items;
    }
}
