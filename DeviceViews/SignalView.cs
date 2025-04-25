using RailwaySignalsModels;
using RailwaySignalsModels.CbiDeviceStatus;
using RailwaySignalsModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary.DeviceViews
{
    public class SignalView : CommandDevice, ISignal
    {
        private bool flashFlag;

        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public SignalModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public SignalStatus Status { get; } = new() { IsLighting = true };

        [PropertyIgnore, XmlAttribute]
        public bool IsLeftSide { get; set; }

        [XmlAttribute]
        public SignalColor DefaultColor { get; set; } = SignalColor.Red;

        [PropertyIgnore, XmlIgnore]
        public override bool IsFlashing => IsSelected || Status.Color == SignalColor.DSFault || Status.Color == SignalColor.Yellow2Flash;

        [PropertyIgnore, XmlIgnore]
        public List<Line> Lines { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public List<Circle> Circles { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public List<Rectangle> Rects { get; set; } = new();

        private readonly Line[] crossLines = new Line[4];
        private Brush defaultBrush;
        protected Rect blockRect;
        protected readonly TextBlock _delayText = new() { Width = 22, Height = 16, Background = null, Foreground = Brushes.White };

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

        public override void AddShape(Shape shape)
        {
            base.AddShape(shape);
            if (shape is Rectangle rect)
                Rects.Add(rect);
        }

        private void CreateCrossLines()
        {
            double xyLength = Math.Cos(Radian(45)) * Circles[0].Radio;
            var cross1 = CreateCrossPoints(Circles[0].Center, xyLength);
            crossLines[0] = cross1[0];
            crossLines[1] = cross1[1];

            if (Circles.Count > 1)
            {
                var cross2 = CreateCrossPoints(Circles[1].Center, xyLength);
                crossLines[2] = cross2[0];
                crossLines[3] = cross2[1];
            }

            var point = Circles[^1].Center;
            point.X += point.X > Lines[0].Pt0.X ? Circles[0].Radio : - Circles[0].Radio;
            point.Y += point.Y > Lines[0].Pt0.Y ? Circles[0].Radio: - Circles[0].Radio;
            blockRect = new Rect(Lines[0].Pt0, point);
        }

        private static Line[] CreateCrossPoints(Point center, double xyLength)
        {
            var lines = new Line[]
            {
                new()
                {
                    Pt0 = new Point(center.X - xyLength, center.Y - xyLength),
                    Pt1 = new Point(center.X + xyLength, center.Y + xyLength)
                },
                new()
                {
                    Pt0 = new Point(center.X + xyLength, center.Y - xyLength),
                    Pt1 = new Point(center.X - xyLength, center.Y + xyLength)
                }
            };

            return lines;
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
            RenderCrossLines(dc);

            if (Status.IsBlocked)
                dc.DrawRectangle(null, LinePen, blockRect);

            RenderName(dc);
        }

        public void RenderCrossLines(DrawingContext dc)
        {
            if (Status.IsLighting || Model.IOType is SignalType.L1 or SignalType.L2_蓝白 or SignalType.L2_红白) return;

            if (Status.Color is SignalColor.Red or SignalColor.White 
                or SignalColor.Green2 or SignalColor.Yellow2 or SignalColor.Yellow2Flash or SignalColor.RedYellow or SignalColor.GreenYellow)
            {
                crossLines[0].Render(dc, LinePen);
                crossLines[1].Render(dc, LinePen);
            }

            if (Status.Color is SignalColor.Red)
                return;

            var lastLineIndex = (Circles.Count - 1) * 2;
            crossLines[lastLineIndex].Render(dc, LinePen);
            crossLines[lastLineIndex + 1].Render(dc, LinePen);
        }

        public override void OnFlashTimer()
        {
            base.OnFlashTimer();
            flashFlag = !flashFlag;
        }

        public void RenderEmptyLights(DrawingContext dc)
        {
            if (Circles.Count == 1) return;

            if (Status.Color is SignalColor.Green or SignalColor.Yellow or SignalColor.Blue)
            {
                Circles[0].Render(dc, LightPen, Brushes.Black);
                return;
            }

            if (Status.Color is SignalColor.RedYellow or SignalColor.Green2 or SignalColor.Yellow2
                or SignalColor.GreenYellow or SignalColor.RedWhite or SignalColor.Yellow2Flash)
                return;

            Circles[1].Render(dc, LightPen, Brushes.Black);
        }

        public void RenderColoredLights(DrawingContext dc)
        {
            defaultBrush ??= DefaultColor == SignalColor.Red ? Brushes.Red : Brushes.Blue;

            switch (Status.Color)
            {
                case SignalColor.DSFault:
                    Circles[0].Render(dc, LightPen, flashFlag ? defaultBrush : Brushes.Black);
                    break;
                case SignalColor.Blue:
                    Circles[^1].Render(dc, LightPen, Brushes.Blue);
                    break;
                case SignalColor.RedYellow:
                    Circles[0].Render(dc, LightPen, Brushes.Red);
                    Circles[1].Render(dc, LightPen, Brushes.Yellow);
                    break;
                case SignalColor.White:
                    Circles[0].Render(dc, LightPen, Brushes.White);
                    break;
                case SignalColor.Yellow:
                    Circles[^1].Render(dc, LightPen, Brushes.Yellow);
                    break;
                case SignalColor.Green:
                    Circles[^1].Render(dc, LightPen, Brushes.Green);
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
                    Circles[0].Render(dc, LightPen, Brushes.White);
                    Circles[1].Render(dc, LightPen, Brushes.Red);
                    break;

                case SignalColor.Yellow2Flash:
                    Circles[0].Render(dc, LightPen, Brushes.Yellow);
                    Circles[1].Render(dc, LightPen, flashFlag ? null : Brushes.Yellow);
                    break;

                //case SignalColor.DS2Fault:
                //case SignalColor.Red:
                //    Circles[0].Render(dc, LightPen, Brushes.Red);
                //    break;
                default:
                    Circles[0].Render(dc, LightPen, Brushes.Red);
                    break;
            }
        }

        public void RenderLines(DrawingContext dc)
        {
            foreach (var line in Lines)
                line.Render(dc, LinePen);
        }

        public virtual void RenderRects(DrawingContext dc)
        {
            foreach (var rect in Rects)
                rect?.Render(dc);
        }

        public TextBlock SetDelayTextPosition()
        {
            Canvas.SetLeft(_delayText, NamePos.X + TopLeft.X + FmtName!.Width + 10);
            Canvas.SetTop(_delayText, NamePos.Y + TopLeft.Y);
            return _delayText;
        }

        protected static Pen LinePen { get; set; } = new(Brushes.White, 3);
        protected static Pen CrossPen { get; set; } = new(Brushes.Red, 3);
        protected static Pen LightPen { get; set; } = new(Brushes.White, 1);

        static public bool ShowRectButtons { get; set; } = true;
    }
}
