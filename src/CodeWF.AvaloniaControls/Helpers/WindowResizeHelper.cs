using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeWF.AvaloniaControls.Helpers;

public class WindowResizeHelper
{
    private bool _isResizing = false;

    private ResizeDirection _resizeDirection;
    private Point _resizeStartPoint;
    private PixelPoint WindowStartPosition;
    private Size WindowStartSize;

    private bool _isMoving = false;

    private Point _mouseStartPosition;
    private PixelPoint _windowStartPosition;

    private const int ResizeBorder = 8;
    private const double MinScaleDelta = 1.0;
    public Window DestWindow { get; init; }

    public WindowResizeHelper(Window window)
    {
        DestWindow = window;

        DestWindow.GetObservable(Window.WindowStateProperty).Subscribe(OnWindowStateChanged);

        DestWindow.MinWidth = 200;
        DestWindow.MinHeight = 150;

        DestWindow.Closed += (s, e) => 
        {
            DisenableResizing();
        };
    }

    public void EnableResizing()
    {
        DestWindow.PointerMoved += OnPointerMoved;
        DestWindow.PointerPressed += OnPointerPressed;
        DestWindow.PointerReleased += OnPointerReleased;
        DestWindow.PointerCaptureLost += OnPointerCaptureLost;
    }

    public void DisenableResizing()
    {
        DestWindow.PointerMoved -= OnPointerMoved;
        DestWindow.PointerPressed -= OnPointerPressed;
        DestWindow.PointerReleased -= OnPointerReleased;
        DestWindow.PointerCaptureLost -= OnPointerCaptureLost;
    }

    private double GetScalingFactor()
    {
        var scaling = DestWindow.GetVisualRoot()?.RenderScaling;
        if(scaling is not null)
        {
            return Math.Round(scaling.Value, 2);
        }
        return 1.0;
    }

    private void OnWindowStateChanged(WindowState state)
    {
        if (state != WindowState.Maximized)
        {
            _isResizing = false;
            _isMoving = false;
        }
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if(DestWindow.WindowState == WindowState.Maximized)
        {
            DestWindow.Cursor = Cursor.Default;
            return;
        }

        var point = e.GetPosition(DestWindow);
        var scaling = GetScalingFactor();

        if(_isMoving)
        {
            var delta = point - _mouseStartPosition;
            var pixelDeltaX = (int)Math.Round(delta.X * scaling);
            var pixelDeltaY = (int)Math.Round(delta.Y * scaling);
            DestWindow.Position = new PixelPoint(
                WindowStartPosition.X + pixelDeltaX,
                WindowStartPosition.Y + pixelDeltaX
            );
            return;
        }

        if(_isResizing)
        {
            var delta = point - _resizeStartPoint;
            if(Math.Abs(delta.X) < MinScaleDelta && Math.Abs(delta.Y) < MinScaleDelta)
            {
                return;
            }

            var newWidth = WindowStartSize.Width;
            var newHeight = WindowStartSize.Height;
            var newX = (double)WindowStartPosition.X;
            var newY = (double)WindowStartPosition.Y;

            switch(_resizeDirection)
            {
                case ResizeDirection.Left:
                    {
                        newWidth = WindowStartSize.Width - delta.X;
                        if(newWidth >= DestWindow.MinWidth)
                        {
                            newX = WindowStartPosition.X + (delta.X * scaling);
                        }
                        else
                        {
                            newWidth = DestWindow.MinWidth;
                            newX = WindowStartPosition.X + (WindowStartSize.Width - newWidth) * scaling;
                        }
                    }
                    break;
                case ResizeDirection.Top:
                    {
                        newHeight = WindowStartSize.Height - delta.Y;
                        if (newHeight >= DestWindow.MinHeight)
                        {
                            newY = WindowStartPosition.Y + (delta.Y * scaling);
                        }
                        else
                        {
                            newHeight = DestWindow.MinHeight;
                            newY = WindowStartPosition.Y + (WindowStartSize.Height - newHeight) * scaling;
                        }
                    }
                    break;
                case ResizeDirection.TopLeft:
                    {
                        newWidth = WindowStartSize.Width - delta.X;
                        if (newWidth >= DestWindow.MinWidth)
                        {
                            newX = WindowStartPosition.X + (delta.X * scaling);
                        }
                        else
                        {
                            newWidth = DestWindow.MinWidth;
                            newX = WindowStartPosition.X + (WindowStartSize.Width - newWidth) * scaling;
                        }

                        newHeight = WindowStartSize.Height - delta.Y;
                        if (newHeight >= DestWindow.MinHeight)
                        {
                            newY = WindowStartPosition.Y + (delta.Y * scaling);
                        }
                        else
                        {
                            newHeight = DestWindow.MinHeight;
                            newY = WindowStartPosition.Y + (WindowStartSize.Height - newHeight) * scaling;
                        }
                    }
                    break;
                case ResizeDirection.Right:
                    newWidth = Math.Max(WindowStartSize.Width + delta.X, DestWindow.MinWidth);
                    break;
                case ResizeDirection.Bottom:
                    newHeight = Math.Max(WindowStartSize.Height + delta.Y, DestWindow.MinHeight);
                    break;
                case ResizeDirection.TopRight:
                    {
                        newWidth = Math.Max(WindowStartSize.Width + delta.X, DestWindow.MinWidth);
                        newHeight = WindowStartSize.Height - delta.Y;
                        if (newHeight >= DestWindow.MinHeight)
                        {
                            newY = WindowStartPosition.Y + (delta.Y * scaling);
                        }
                        else
                        {
                            newHeight = DestWindow.MinHeight;
                            newY = WindowStartPosition.Y + (WindowStartSize.Height - newHeight) * scaling;
                        }
                    }
                    break;
                case ResizeDirection.BottomLeft:
                    {
                        newWidth = WindowStartSize.Width - delta.X;
                        if (newWidth >= DestWindow.MinWidth)
                        {
                            newX = WindowStartPosition.X + (delta.X * scaling);
                        }
                        else
                        {
                            newWidth = DestWindow.MinWidth;
                            newX = WindowStartPosition.X + (WindowStartSize.Width - newWidth) * scaling;
                        }
                        newHeight = Math.Max(WindowStartSize.Height + delta.Y, DestWindow.MinHeight);
                    }
                    break;
                case ResizeDirection.BottomRight:
                    {
                        newWidth = Math.Max(WindowStartSize.Width + delta.X, DestWindow.MinWidth);
                        newHeight = Math.Max(WindowStartSize.Height + delta.Y, DestWindow.MinHeight);
                    }
                    break;
            }

            DestWindow.Width = Math.Round(newWidth);
            DestWindow.Height = Math.Round(newHeight);
            DestWindow.Position = new PixelPoint((int)Math.Round(newX), (int)Math.Round(newY));
            return;
        }

        var direction = GetResizeDirection(point);
        DestWindow.Cursor = direction != ResizeDirection.None ? GetCursorForDirection(direction) : Cursor.Default;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(sender as Window).Properties.IsLeftButtonPressed) return;
        if (DestWindow.WindowState == WindowState.Maximized) return;

        var point = e.GetPosition(DestWindow);
        var resizeDirection = GetResizeDirection(point);

        if(resizeDirection == ResizeDirection.Bottom || resizeDirection == ResizeDirection.Right || resizeDirection == ResizeDirection.BottomRight)
        {
            _isResizing = true;
            _resizeDirection = resizeDirection;
            _resizeStartPoint = point;

            WindowStartSize = new Size(Math.Round(DestWindow.Width), Math.Round(DestWindow.Height));
            WindowStartPosition = DestWindow.Position;
            e.Pointer.Capture(DestWindow);
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isResizing = false;
        _isMoving = false;
        e.Pointer.Capture(default);
    }

    private void OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        _isResizing = false;
        _isMoving = false;
    }

    private ResizeDirection GetResizeDirection(Point point)
    {
        var width = Math.Round(DestWindow.Width);
        var height = Math.Round(DestWindow.Height);

        var isLeft = point.X <= ResizeBorder;
        var isRight = point.X >= (width - ResizeBorder);
        var isTop = point.Y <= ResizeBorder;
        var isBottom = point.Y >= (height - ResizeBorder);

        if (isLeft && isTop) return ResizeDirection.TopLeft;
        if (isRight && isTop) return ResizeDirection.TopRight;
        if (isLeft && isBottom) return ResizeDirection.BottomLeft;
        if (isRight && isBottom) return ResizeDirection.BottomRight;
        if (isLeft) return ResizeDirection.Left;
        if (isRight) return ResizeDirection.Right;
        if (isTop) return ResizeDirection.Top;
        if (isBottom ) return ResizeDirection.Bottom;
         return ResizeDirection.None;
    }
    private Cursor GetCursorForDirection(ResizeDirection direction)
    {
        return direction switch
        {
            ResizeDirection.Left or ResizeDirection.Right => new Cursor(StandardCursorType.SizeWestEast),
            ResizeDirection.Top or ResizeDirection.Bottom => new Cursor(StandardCursorType.SizeNorthSouth),
            ResizeDirection.TopLeft  => new Cursor(StandardCursorType.TopLeftCorner),
            ResizeDirection.TopRight  => new Cursor(StandardCursorType.TopRightCorner),
            ResizeDirection.BottomLeft => new Cursor(StandardCursorType.BottomLeftCorner),
            ResizeDirection.BottomRight => new Cursor(StandardCursorType.BottomRightCorner),
            _ => Cursor.Default,
        };
    }
}

public enum ResizeDirection
{
    None,
    Left,
    Right,
    Top,
    Bottom,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}