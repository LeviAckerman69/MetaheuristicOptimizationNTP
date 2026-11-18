using MetaheuristicOptimizationNTP.Structures;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MetaheuristicOptimizationNTP.Helper;

public static class RoutePreviewHelper
{
    public static byte[] CreatePng(IReadOnlyList<Town> towns, Solution solution)
    {
        const int width = 800;
        const int height = 500;
        const double padding = 35;
        var minX = towns.Min(town => town.X);
        var minY = towns.Min(town => town.Y);
        var rangeX = towns.Max(town => town.X) - minX;
        var rangeY = towns.Max(town => town.Y) - minY;
        var scale = Math.Min((width - padding * 2) / Math.Max(rangeX, 1),
            (height - padding * 2) / Math.Max(rangeY, 1));
        var offsetX = (width - rangeX * scale) / 2;
        var offsetY = (height - rangeY * scale) / 2;
        var points = towns.Select(town => new Point(
            (town.X - minX) * scale + offsetX,
            (town.Y - minY) * scale + offsetY)).ToList();

        var visual = new DrawingVisual();
        using (var drawing = visual.RenderOpen())
        {
            drawing.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
            var order = solution.PermutationView;
            for (var i = 0; i < order.Count; i++)
            {
                drawing.DrawLine(new Pen(Brushes.Black, 2), points[order[i]], points[order[(i + 1) % order.Count]]);
            }
            for (var i = 0; i < towns.Count; i++)
            {
                var point = points[i];
                drawing.DrawEllipse(Brushes.Red, new Pen(Brushes.Black, 1), point, Town.Radius, Town.Radius);
                var label = new FormattedText(towns[i].Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface("Segoe UI"), 10, Brushes.White, 1);
                var availableWidth = Town.Radius * 2 - 6;
                if (label.Width > availableWidth)
                {
                    label.SetFontSize(10 * availableWidth / label.Width);
                }
                drawing.DrawText(label, new Point(point.X - label.Width / 2, point.Y - label.Height / 2));
            }
        }

        var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(visual);
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }
}
