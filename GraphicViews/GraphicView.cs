using GraphicsWpfLibrary.DeviceViews;
using RailwaySignalsModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class GraphicView
    {
        [XmlIgnore, PropertyIgnore]
        public UIRender UI { get; }

        [XmlIgnore]
        public DrawingGroup Drawing { get; } = new();

        [XmlAttribute]
        public double RotationAngle
        {
            get => (UI.RenderTransform is RotateTransform rotation) ? rotation.Angle : 0;
            set => UI.RenderTransform = new RotateTransform(value, 0, 0);
        }

        [XmlIgnore]
        public virtual Point TopLeft
        {
            get => new(Canvas.GetLeft(UI), Canvas.GetTop(UI));
            set
            {
                Canvas.SetLeft(UI, value.X);
                Canvas.SetTop(UI, value.Y);
            }
        }

        [XmlAttribute, PropertyIgnore]
        public string STopLeft
        {
            get => $"{TopLeft.X:F2},{TopLeft.Y:F2}";
            set => TopLeft = Point.Parse(value);
        }

        [XmlElement("Line", typeof(Line))]
        [XmlElement("Circle", typeof(Circle))]
        [XmlElement("Rectangle", typeof(Rectangle))]
        public List<Shape> Shapes { get; set; }

        [PropertyIgnore, XmlIgnore]
        public virtual bool IsSelected { get; set; }

        [PropertyIgnore]
        public Action<DrawingContext> AdditionalRenderAction;

        public GraphicView() => UI = new() { Graphic = this };

        public virtual void Render() { }

        public void RenderDefaultShpaes(DrawingContext dc)
        {
            Pen pen = IsSelected ? SectionView.ProtectPen : SectionView.ClearPen;

            foreach (var shape in Shapes)
                shape.Render(dc, pen, Brushes.Black);
        }

        public bool InRange(Point checkPoint, ref Point connectPoint, double range)
        {
            if (Shapes != null)
            {
                foreach (var shape in Shapes)
                    if (shape is Line line)
                        if (line.InRange(checkPoint, ref connectPoint, range))
                            return true;
            }
            return false;
        }
    }
}