using GraphicsWpfLibrary.DeviceViews;
using RailwaySignalsModels;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary.GraphicViews
{
    public class ChangeDirectionPanelView : GraphicView, IContainerView
    {
        private FormattedText _formattedName;

        [XmlAttribute]
        public string Name
        {
            get => _formattedName is null ? string.Empty : _formattedName.Text;
            set
            {
                CreateFormattedName(value);
                if (Shapes.Count > 0) 
                    SetNamePosition();

                if (!RenderBorder)
                {
                    foreach (var button in Buttons)
                        button.DeviceInfo.Name = value + button.DeviceInfo.Name;

                    foreach (var light in StatusLights)
                        light.DeviceInfo.Name = value + light.DeviceInfo.Name;
                }
            }
        }

        [XmlIgnore]
        public Point NamePos { get; set; }

        [PropertyIgnore, XmlAttribute]
        public string SNamePos
        {
            get => $"{NamePos.X:F2},{NamePos.Y:F2}";
            set => NamePos = Point.Parse(value);
        }

        [PropertyIgnore]
        public StatusLightView[] StatusLights { get; set; }

        [PropertyIgnore]
        public DeviceButtonView[] Buttons { get; set; }

        [XmlIgnore]
        public override Point TopLeft
        {
            get => base.TopLeft;
            set
            {
                var offset = value - base.TopLeft;
                if (Buttons is not null)
                {
                    foreach (var door in Buttons)
                        if (door != null)
                            door.TopLeft += offset;
                }

                if (StatusLights is not null)
                {
                    foreach (var button in StatusLights)
                        if (button != null)
                            button.TopLeft += offset;
                }

                base.TopLeft = value;
            }
        }

        [XmlAttribute]
        public bool RenderBorder { get; set; } = true;

        public override void Render()
        {
            using var dc = Drawing.Open();

            if (RenderBorder)
                Shapes[0].Render(dc, RectPen);
            else
            {
                Shapes[0].Render(dc);
                return;
            }

            if (_formattedName == null) return;

            var x = ((Shapes[0] as Rectangle).Rect.Width - _formattedName.Width) / 2 -2;
            var nameBackgroundRect = new Rect(x, -_formattedName.Height - 2, _formattedName.Width + 4, _formattedName.Height + 4);

            dc.DrawRectangle(Brushes.Black, null, nameBackgroundRect);
            dc.DrawText(_formattedName, NamePos);
        }

        public void SetNamePosition()
        {
            var rect = Shapes[0] as Rectangle;
            NamePos = new Point((rect.Rect.Width - _formattedName.Width) / 2, -_formattedName.Height / 2);
            UI.InvalidateVisual();
        }

        private void CreateFormattedName(string name, int fontSize = 13, double pixelPerDip = 1)
        {
            _formattedName = new FormattedText(
                    name,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("微软雅黑"), fontSize, Brushes.Silver, pixelPerDip);

            UI.InvalidateVisual();
        }

        public void AddChildren2Canvas(Canvas canvas)
        {
            foreach (var button in Buttons)
                canvas.Children.Add(button.UI);

            foreach (var light in StatusLights)
                canvas.Children.Add(light.UI);

            Panel.SetZIndex(UI, -1);
        }

        public void AddChildren2Models(CbiModel cbiModel)
        {
            if (Buttons is not null)
                foreach (var item in Buttons)
                    cbiModel.Devices.Add(item.DeviceInfo);

            if (StatusLights is not null)
                foreach (var item in StatusLights)
                    cbiModel.Devices.Add(item.DeviceInfo);
        }

        public void RemoveFromGrahics(List<GraphicView> graphics)
        {
            if (Buttons is not null)
                foreach (var item in Buttons)
                    graphics.Remove(item);

            if (StatusLights is not null)
                foreach (var item in StatusLights)
                    graphics.Remove(item);
        }

        public static Pen RectPen { get; } = new Pen(Brushes.Silver, 1);
    }
}
