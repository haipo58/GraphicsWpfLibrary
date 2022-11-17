using RailwaySignalsModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class SignalView : CommandDevice, ISignal
    {
        private bool flashFlag;

        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public SignalModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public SignalStatus Status { get; } = new();

        [PropertyIgnore, XmlAttribute]
        public bool IsLeftSide { get; set; }

        [XmlAttribute]
        public SignalColor DefaultColor { get; set; } = SignalColor.Red;

        public void LoadFromShapes()
        {
            foreach (var shape in Shapes)
            {
                if (shape is Line line)
                    Lines.Add(line);

                if (shape is Circle circle)
                    Circles.Add(circle);

                if (shape is Rectangle rect)
                    Rects.Add(rect);
            }

            Status.Color = DefaultColor;
            CreateCrossLines();
        }

        [PropertyIgnore, XmlIgnore]
        public override bool IsFlashing => IsSelected || Status.Color == SignalColor.DSFault;

        [PropertyIgnore, XmlIgnore]
        public List<Line> Lines { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public List<Circle> Circles { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public List<Rectangle> Rects { get; set; } = new();

        private readonly Line[] crossLines = new Line[2];
        private Rect blockRect;
        private Brush defaultBrush;

        public override void AddShape(Shape shape)
        {
            base.AddShape(shape);
            if (shape is Rectangle rect)
                Rects.Add(rect);
        }

        private void CreateCrossLines()
        {
            Point center = Circles[0].Center;
            double xyLength = Math.Cos(Radian(45)) * Circles[0].Radio;

            crossLines[0] = new Line() 
            { 
                Pt0 = new Point(center.X - xyLength, center.Y - xyLength),
                Pt1 = new Point(center.X + xyLength, center.Y + xyLength)
            };

            crossLines[1] = new Line()
            {
                Pt0 = new Point(center.X + xyLength, center.Y - xyLength),
                Pt1 = new Point(center.X - xyLength, center.Y + xyLength)
            };

            var point = Circles[^1].Center;
            point.X += point.X > Lines[0].Pt0.X ? Circles[0].Radio : - Circles[0].Radio;
            point.Y += point.Y > Lines[0].Pt0.Y ? Circles[0].Radio: - Circles[0].Radio;
            blockRect = new Rect(Lines[0].Pt0, point);
        }

        private static double Radian(double angle) => angle / 180 * Math.PI;

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            RenderLines(dc);

            if (ShowRectButtons)
                RenderRects(dc);

            RenderColoredLights(dc);
            RenderEmptyLights(dc);

            if (!Status.IsLighting && !(Model.IOType == SignalType.L1 || Model.IOType == SignalType.L2_蓝白))
                RenderCrossLines(dc);

            if (Status.IsBlocked)
                dc.DrawRectangle(null, LinePen, blockRect);

            RenderName(dc);
        }

        private void RenderCrossLines(DrawingContext dc)
        {
            crossLines[0].Render(dc, LinePen);
            crossLines[1].Render(dc, LinePen);
        }

        public override void OnFlashTimer()
        {
            base.OnFlashTimer();
            flashFlag = !flashFlag;
        }

        private void RenderEmptyLights(DrawingContext dc)
        {
            if (Circles.Count > 1 &&
                Status.Color != SignalColor.RedYellow
                && Status.Color != SignalColor.Green2
                && Status.Color != SignalColor.Yellow2
                && Status.Color != SignalColor.GreenYellow
                && Status.Color != SignalColor.RedWhite)

                Circles[1].Render(dc, LightPen, Brushes.Black);
        }

        private void RenderColoredLights(DrawingContext dc)
        {
            if (defaultBrush == null)
                defaultBrush = DefaultColor == SignalColor.Red ? Brushes.Red : Brushes.Blue;

            switch (Status.Color)
            {
                case SignalColor.DSFault:
                        Circles[0].Render(dc, LightPen, flashFlag ? defaultBrush : Brushes.Black);
                    break;
                case SignalColor.DS2Fault:
                case SignalColor.Red:
                    Circles[0].Render(dc, LightPen, Brushes.Red);
                    break;
                case SignalColor.Blue:
                    Circles[0].Render(dc, LightPen, Brushes.Blue);
                    break;
                case SignalColor.RedYellow:
                    Circles[0].Render(dc, LightPen, Brushes.Red);
                    Circles[1].Render(dc, LightPen, Brushes.Yellow);
                    break;
                case SignalColor.White:
                    Circles[0].Render(dc, LightPen, Brushes.White);
                    break;
                case SignalColor.Yellow:
                    Circles[0].Render(dc, LightPen, Brushes.Yellow);
                    break;
                case SignalColor.Green:
                    Circles[0].Render(dc, LightPen, Brushes.Green);
                    break;
                case SignalColor.Green2:
                    Circles[0].Render(dc, LightPen, Brushes.Green);
                    Circles[1].Render(dc, LightPen, Brushes.Green);
                    break;
                case SignalColor.Yellow2:
                    Circles[0].Render(dc, LightPen, Brushes.Yellow);
                    Circles[1].Render(dc, LightPen, Brushes.Yellow);
                    break;
                case SignalColor.GreenYellow:
                    Circles[0].Render(dc, LightPen, Brushes.Green);
                    Circles[1].Render(dc, LightPen, Brushes.Yellow);
                    break;
                case SignalColor.RedWhite:
                    Circles[0].Render(dc, LightPen, Brushes.Red);
                    Circles[1].Render(dc, LightPen, Brushes.White);
                    break;
                default:
                    break;
            }
        }

        private void RenderLines(DrawingContext dc)
        {
            foreach (var line in Lines)
                line.Render(dc, LinePen);
        }

        public virtual void RenderRects(DrawingContext dc)
        {
            foreach (var rect in Rects)
                rect.Render(dc, LightPen);
        }

        protected static Pen LinePen { get; set; } = new(Brushes.White, 3);
        protected static Pen CrossPen { get; set; } = new(Brushes.Red, 3);
        protected static Pen LightPen { get; set; } = new(Brushes.White, 1);

        static public bool ShowRectButtons { get; set; } = true;
    }
}
