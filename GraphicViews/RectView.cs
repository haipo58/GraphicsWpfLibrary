using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class RectView : GraphicView
    {
        [XmlIgnore, PropertyIgnore]
        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                UI.InvalidateVisual();
            }
        }

        public override void Render(DrawingContext dc)
            => Shapes[0].Render(dc, IsSelected? SelectedPen : DeviceButtonView.BorderPen);

        private static readonly Pen SelectedPen = new Pen(Brushes.Yellow, 1);
    }
}
