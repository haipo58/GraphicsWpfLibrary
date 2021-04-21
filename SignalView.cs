using RailwaySignalsModels;
using System;
using System.Collections.Generic;
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

        public void LoadFromShapes()
        {
            foreach (var shape in Shapes)
            {
                if (shape is Line line)
                    Lines.Add(line);

                if (shape is Circle circle)
                    Circles.Add(circle);
            }
        }

        [PropertyIgnore, XmlIgnore]
        public override bool IsFlashing => IsSelected || Status.Color == SignalColor.DSFault;

        [PropertyIgnore, XmlIgnore]
        public List<Line> Lines { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public List<Circle> Circles { get; } = new();

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            RenderLines(dc);
            RenderColoredLights(dc);
            RenderEmptyLights(dc);
            RenderName(dc);
        }

        public override void OnFlashTimer()
        {
            base.OnFlashTimer();
            flashFlag = !flashFlag;
        }

        private void RenderEmptyLights(DrawingContext dc)
        {
            if (Circles.Count > 1 && Status.Color != SignalColor.RedYellow)
                Circles[1].Render(dc, lightPen, Brushes.Black);
        }

        private void RenderColoredLights(DrawingContext dc)
        {
            switch (Status.Color)
            {
                case SignalColor.DSFault:
                        Circles[0].Render(dc, lightPen, flashFlag ? Brushes.Red : Brushes.Black);
                    break;
                case SignalColor.DS2Fault:
                case SignalColor.Red:
                    Circles[0].Render(dc, lightPen, Brushes.Red);
                    break;
                case SignalColor.Blue:
                    Circles[0].Render(dc, lightPen, Brushes.Blue);
                    break;
                case SignalColor.RedYellow:
                    Circles[0].Render(dc, lightPen, Brushes.Red);
                    Circles[1].Render(dc, lightPen, Brushes.Yellow);
                    break;
                case SignalColor.White:
                    Circles[0].Render(dc, lightPen, Brushes.White);
                    break;
                case SignalColor.Yellow:
                    Circles[0].Render(dc, lightPen, Brushes.Yellow);
                    break;
                case SignalColor.Green:
                    Circles[0].Render(dc, lightPen, Brushes.Green);
                    break;
                default:
                    break;
            }
        }

        private void RenderLines(DrawingContext dc)
        {
            foreach (var line in Lines)
                line.Render(dc, linePen);
        }

        private static readonly Pen linePen = new(Brushes.White, 3);
        private static readonly Pen lightPen = new(Brushes.White, 1);
    }
}
