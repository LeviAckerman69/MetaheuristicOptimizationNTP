using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement
{
    private Town? _draggedTown;
    private Vector _dragOffset;
    private Point _dragPosition;

    private static MockViewModel MockViewModel { get; } = new();

    private IViewModel ViewModel => DataContext as IViewModel ?? MockViewModel;

    public TownDisplay()
    {
        Loaded += (s, e) =>
        {
            ViewModel.Towns.CollectionChanged += (s, e) => InvalidateVisual();
            (ViewModel as ObservableObject)?.PropertyChanged += (s, e) => InvalidateVisual();
        };
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

        DrawPaths(drawingContext);

        DrawTowns(drawingContext);
    }

    private void DrawPaths(DrawingContext drawingContext)
    {
        var selectedSolution = ViewModel.SelectedSolution;

        var towns = ViewModel.Towns;

        if (selectedSolution is not null)
        {
            var permutation = selectedSolution.PermutationView;

            for (var i = 0; i < permutation.Count; i++)
            {
                var townId1 = permutation[i];
                var town1 = towns[townId1];
                var point1 = GetDisplayPosition(town1);

                var townId2 = permutation[(i + 1) % permutation.Count];
                var town2 = towns[townId2];
                var point2 = GetDisplayPosition(town2);

                drawingContext.DrawLine(new Pen(Brushes.Black, 2), point1, point2);
            }
        }
    }

    private void DrawTowns(DrawingContext drawingContext)
    {
        var towns = ViewModel.Towns;

        foreach (var town in towns)
        {
            var point = GetDisplayPosition(town);
            var brush = Brushes.Red;
            var pen = new Pen(Brushes.Black, 2);
            drawingContext.DrawEllipse(brush, pen, point, Town.Radius, Town.Radius);

            var label = new FormattedText(town.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Segoe UI"), 10, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
            var availableWidth = Town.Radius * 2 - 6;
            if (label.Width > availableWidth)
            {
                label.SetFontSize(10 * availableWidth / label.Width);
            }

            drawingContext.DrawText(label, new Point(point.X - label.Width / 2, point.Y - label.Height / 2));
        }
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        var position = e.GetPosition(this);
        var town = ViewModel.Towns.LastOrDefault(town => town.Contains(position));
        if (town is null)
        {
            ViewModel.AddNewTownAtPosition(position);
            return;
        }

        _draggedTown = town;
        _dragOffset = position - town.Point;
        _dragPosition = town.Point;
        if (!CaptureMouse())
        {
            _draggedTown = null;
        }
        e.Handled = true;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_draggedTown is null)
        {
            return;
        }

        var position = e.GetPosition(this) - _dragOffset;
        _dragPosition = new Point(
            Math.Clamp(position.X, Town.Radius, Math.Max(Town.Radius, ActualWidth - Town.Radius)),
            Math.Clamp(position.Y, Town.Radius, Math.Max(Town.Radius, ActualHeight - Town.Radius)));
        InvalidateVisual();
        e.Handled = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        if (_draggedTown is null)
        {
            return;
        }

        var town = _draggedTown;
        var position = _dragPosition;
        _draggedTown = null;
        ReleaseMouseCapture();
        ViewModel.MoveTownToPosition(town, position);
        InvalidateVisual();
        e.Handled = true;
    }

    protected override void OnLostMouseCapture(MouseEventArgs e)
    {
        base.OnLostMouseCapture(e);
        // Losing capture cancels the preview without changing the stored coordinates.
        _draggedTown = null;
        InvalidateVisual();
    }

    private Point GetDisplayPosition(Town town) => town == _draggedTown ? _dragPosition : town.Point;

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);
        if (_draggedTown is not null)
        {
            return;
        }
        var position = e.GetPosition(this);
        ViewModel.RemoveTownAtPosition(position);
    }
}
