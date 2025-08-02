using System;
using Ursa.Controls;

namespace CodeWF.Demo.Views
{
    public partial class MainWindow : UrsaWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AdjustWindowSize();
        }

        private void AdjustWindowSize()
        {
            if (Screens.Primary is not { } screen)
            {
                return;
            }

            const double resolutionThreshold = 1920 + 50;
            var isSmaller = screen.WorkingArea.Width < resolutionThreshold;
            var targetWidth = isSmaller ? 1440 : 1920;
            var targetHeight = isSmaller ? 810 : 1080;
            MinWidth = Width = Math.Min(targetWidth, screen.WorkingArea.Width);
            MinHeight = Height = Math.Min(targetHeight, screen.WorkingArea.Height);
        }
    }
}