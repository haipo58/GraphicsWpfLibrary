using RailwaySignalsModels;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicsWpfLibrary.GraphicViews
{
    public class BoxView : GraphicView, IContainerView
    {
        private FormattedText _formattedName;

        public string Name
        {
            get => _formattedName is null ? string.Empty : _formattedName.Text;
            set => _formattedName = CreateFormattedName(value);
        }

        public Point NamePos { get; set; }

        public void SetNamePosition()
        {
            var rect = Shapes[0] as Rectangle;
            NamePos = new Point((rect.Rect.Width - _formattedName.Width) / 2, -_formattedName.Height / 2);
            UI.InvalidateVisual();
        }

        public static FormattedText CreateFormattedName(string name, int fontSize = 13, double pixelPerDip = 1)
        {
            return new FormattedText(
                    name,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("微软雅黑"), fontSize, Brushes.Silver, pixelPerDip);
        }

        public override void Render(DrawingContext dc)
        {
            Shapes[0].Render(dc, ChangeDirectionPanelView.RectPen);

            if (_formattedName == null) return;

            var x = ((Shapes[0] as Rectangle).Rect.Width - _formattedName.Width) / 2 - 2;
            var nameBackgroundRect = new Rect(x, -_formattedName.Height - 2, _formattedName.Width + 4, _formattedName.Height + 4);

            dc.DrawRectangle(Brushes.Black, null, nameBackgroundRect);
            dc.DrawText(_formattedName, NamePos);
        }

        public void AddChildren2Canvas(Canvas canvas)
        {
        }

        public void AddChildren2Models(CbiModel cbiModel)
        {
        }

        public void RemoveFromGrahics(List<GraphicView> graphics)
        {
        }
    }
}
