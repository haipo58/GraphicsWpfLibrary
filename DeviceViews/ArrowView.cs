using RailwaySignalsModels;
using RailwaySignalsModels.CbiDeviceStatus;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary.DeviceViews
{
    public class ArrowView : DeviceView
    {
        [PropertyShow]
        public override DeviceModel DeviceInfo { get; set; } = new DeviceModel();

        [PropertyIgnore, XmlIgnore]
        public ArrowStatus Status { get; set; } = new();
        public bool IsArriveLeft { get; set; }

        private readonly Point[] leftPoints;
        private readonly Point[] rightPoints;
        private readonly Rect rect;

        public ArrowView()
        {
            const int width = 24;
            const int height = 8;
            leftPoints = new Point[3]
            {
                new(0, height),
                new(height, height * 2),
                new(height, 0)
            };

            rightPoints = new Point[3]
            {
                new(height * 2 + width, height),
                new(height + width, height * 2),
                new(height + width, 0),
            };

            rect = new Rect(height, height / 2, width, height);
        }

        public override void Render()
        {
            using var dc = Drawing.Open();
            var brush = Status.Direction switch
            {
                ArrowDirection.Arrive => Brushes.Yellow,
                ArrowDirection.Depart => Brushes.Green,
                _ => Brushes.Black
            };

            if (Status.IsOccupied) brush = Brushes.Red;

            var pen = Status.Direction == ArrowDirection.None ? NonDirectionPen : null;
            dc.DrawRectangle(brush, pen, rect);

            if (Status.Direction != ArrowDirection.None)
            {
                if (IsArriveLeft)
                    DrawTriangle(dc, brush, Status.Direction == ArrowDirection.Arrive ? leftPoints : rightPoints);
                else
                    DrawTriangle(dc, brush, Status.Direction == ArrowDirection.Arrive ? rightPoints : leftPoints);
            }
        }

        private static void DrawTriangle(DrawingContext dc, Brush brush, Point[] points)
        {
            var lineSegment = new PolyLineSegment(points, false);
            var pathFigure = new PathFigure(points[0], new PathSegment[] { lineSegment }, true);
            var geometry = new PathGeometry(new PathFigure[] { pathFigure });
            dc.DrawGeometry(brush, null, geometry);
        }

        static readonly Pen NonDirectionPen = new(Brushes.Silver, 1);
    }
}
