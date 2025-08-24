using Avalonia;
using Avalonia.VisualTree;

namespace CodeWF.Helpers;

public static class ControlHelper
{
    public static T? FindAncestor<T>(this Visual visual) where T : Visual
    {
        var parent = visual;
        while (parent != null)
        {
            parent = parent.GetVisualParent();
            if (parent is T typedParent)
            {
                return typedParent;
            }
        }

        return null;
    }
}