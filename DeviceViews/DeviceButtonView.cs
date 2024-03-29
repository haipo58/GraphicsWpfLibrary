﻿using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class DeviceButtonView : CommandDevice
    {
        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public DeviceButtonModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public RelayStatus Status { get; } = new();

        [XmlIgnore]
        public Brush ToggleBrush { get; set; } = Brushes.Gray;

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);

            (Shapes[0] as Rectangle).Render(dc, BorderPen, Status.IsUp ? Brushes.Gray : ToggleBrush);
            RenderName(dc);
        }

        public static readonly Pen BorderPen = new(Brushes.White, 1);
    }
}
