using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWF.AvaloniaControls.Demo.ViewModels
{
    internal class VComboBoxDemoViewModel
    {
        public List<string> Numbers { get; } = new List<string>() { "正常", "告警", "异常" };
    }
}