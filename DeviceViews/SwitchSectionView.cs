using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary.DeviceViews;

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
    public SectionStatus Status { get; } = new ();

    public override void Render()
    {
        using var dc = Drawing.Open();
        RenderName(dc);
    }
}
