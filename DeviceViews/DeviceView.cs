﻿using RailwaySignalsModels;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class DeviceView : GraphicView
    {
        [PropertyIgnore, XmlIgnore]
        public FormattedText FmtName { get; private set; }

        [XmlIgnore]
        public Point NamePos { get; set; }

        [PropertyIgnore, XmlAttribute]
        public string SNamePos
        {
            get => $"{NamePos.X:F2},{NamePos.Y:F2}";
            set => NamePos = Point.Parse(value);
        }

        [PropertyIgnore, XmlIgnore]
        public virtual DeviceModel DeviceInfo { get; set; }

        public void CreateFormattedName(int fontSize = 13, double pixelPerDip = 1)
        {
            FmtName = new FormattedText(
                    DeviceInfo.Name,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("微软雅黑"), fontSize, Brushes.Silver, pixelPerDip);

            UI.InvalidateVisual();
        }

        public void RenderName(DrawingContext dc, Brush foreBrush)
        {
            if (FmtName == null)
                CreateFormattedName();

            FmtName.SetForegroundBrush(foreBrush);
            dc.DrawText(FmtName, NamePos);
        }

        public override string ToString() => $"{DeviceInfo.Name}";

        public virtual void AddShape(Shape shape) => Shapes.Add(shape);
    }
}