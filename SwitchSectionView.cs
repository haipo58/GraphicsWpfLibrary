using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class SwitchSectionView : CommandDevice, ISection
    {
        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyIgnore, XmlAttribute]
        public bool IsUpSwitch { get; set; }

        [PropertyShow]
        [XmlElement("SwitchSectionModel", typeof(SwitchSectionModel))]
        public SectionModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public SectionStatus Status { get; } = new();

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            RenderName(dc);
        }
    }
}
