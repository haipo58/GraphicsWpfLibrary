using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class CommandDevice : DeviceView, IFlashItem
    {
        [PropertyIgnore, XmlIgnore]
        public bool IsNameVisable { get; set; } = true;

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

        [PropertyIgnore, XmlIgnore]
        public Brush ForeBrush { get; set; } = Brushes.Silver;

        public virtual void OnFlashTimer() => NameFlashFlag = !NameFlashFlag;

        public virtual void RenderName(DrawingContext dc)
        {
            if (!IsNameVisable)
                return;

            var brush = NameFlashFlag && IsSelected ? Brushes.DarkSlateGray : ForeBrush;
            RenderName(dc, brush);
        }
    }
}