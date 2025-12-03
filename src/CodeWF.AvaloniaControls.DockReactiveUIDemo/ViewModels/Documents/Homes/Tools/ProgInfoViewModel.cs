using Avalonia.Threading;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Documents.Homes.Tools;
using CodeWF.AvaloniaControls.Extensions;
using Dock.Model.ReactiveUI.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes.Tools;

public class ProgInfoViewModel : Tool
{
    public ProgInfoViewModel()
    {
        Id = nameof(ProgInfoViewModel);
        Title = "程序信息";

        CreateTestData();
    }

    public RangeObservableCollection<FirstItem>? MultipleLevelItems { get; } = new();

    private void CreateTestData()
    {
        Task.Run(() => 
        {
            List<FirstItem> testDatas = [];
            for (var i = 1; i < 6; i++)
            {
                var firstItem = new FirstItem
                {
                    Id = i,
                    Name = $"FirstItem {i}",
                    SecondItems = []
                };
                for (var j = 1; j < 6; j++)
                {
                    var secondItem = new SecondItem
                    {
                        Id = j,
                        Name = $"SecondItem {j}",
                        ThirdItemItems = []
                    };
                    for (var k = 1; k < 6; k++)
                    {
                        var thirdItem = new ThirdItem { Id = k, Name = $"ThirdItem {k}" };
                        secondItem.ThirdItemItems.Add(thirdItem);
                    }

                    firstItem.SecondItems.Add(secondItem);
                }

                testDatas.Add(firstItem);
            }

            Dispatcher.UIThread.Post(() => { 
                MultipleLevelItems?.AddRange(testDatas);
            });
        });
    }
}