using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class CommandDevice : DeviceView, IFlashItem
    {
        [PropertyIgnore, XmlIgnore]
        public bool NameFlashFlag { get; private set; }

        [PropertyIgnore, XmlIgnore]
        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                UI.InvalidateVisual();
            }
        }

        [PropertyIgnore, XmlIgnore]
        public virtual bool IsFlashing => IsSelected;

        public virtual void OnFlashTimer() => NameFlashFlag = !NameFlashFlag;

        public virtual void RenderName(DrawingContext dc)
        {
            var foreBrush = Brushes.Silver;
            if (NameFlashFlag && IsSelected)
                foreBrush = Brushes.DarkSlateGray;

            RenderName(dc, foreBrush);
        }
    }
}