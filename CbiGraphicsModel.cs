using RailwaySignalsModels;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class CbiGraphicsModel
    {
        private static readonly XmlSerializer serializer = new(typeof(CbiGraphicsModel));

        [XmlElement("Section", typeof(SectionView))]
        [XmlElement("SwitchSection", typeof(SwitchSectionView))]
        [XmlElement("Railswitch", typeof(RailswitchView))]
        [XmlElement("Signal", typeof(SignalView))]
        [XmlElement("Line", typeof(LineView))]
        [XmlElement("End", typeof(EndLine))]
        public List<GraphicView> Graphics { get; set; } = new List<GraphicView>();

        public void Save(string fileName)
        {
            ClearRailswitchShapes();
            using var stream = new StreamWriter(fileName);
            var xmlNamespace = new XmlSerializerNamespaces(
                new XmlQualifiedName[] { new XmlQualifiedName("", "") });

            serializer.Serialize(stream, this, xmlNamespace);
            RestoreRailswitchShapes();
        }

        private void ClearRailswitchShapes()
        {
            foreach (var graphic in Graphics)
                if (graphic is RailswitchView railswitch)
                    railswitch.Shapes.Clear();
        }

        private void RestoreRailswitchShapes()
        {
            foreach (var graphic in Graphics)
                if (graphic is RailswitchView railswitch)
                    railswitch.AddToShapes();
        }

        public static CbiGraphicsModel Open(string fileName, Canvas canvas)
        {
            using var stream = new StreamReader(fileName);
            var graphicsModel = serializer.Deserialize(stream) as CbiGraphicsModel;
            graphicsModel.RestoreRailswitchShapes();
            graphicsModel.InitializeGraphics(canvas);

            AddtoModels(new CbiModel(), graphicsModel.Graphics)
                .Initialize();

            return graphicsModel;
        }

        public static CbiModel AddtoModels(CbiModel cbiModel, List<GraphicView> graphics)
        {
            foreach (var graphic in graphics)
                if (graphic is DeviceView device)
                    cbiModel.Devices.Add(device.DeviceInfo);
            return cbiModel;
        }

        private void InitializeGraphics(Canvas canvas)
        {
            foreach (var graphic in Graphics)
            {
                if (graphic is DeviceView device)
                {
                    device.CreateFormattedName();
                    device.DeviceInfo.NameChangedAction += device.CreateFormattedName;

                    if (device is SignalView signal)
                        signal.LoadFromShapes();
                }

                canvas.Children.Add(graphic.UI);
            }
        }
    }
}