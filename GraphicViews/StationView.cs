using GraphicsWpfLibrary.GraphicViews;
using RailwaySignalsModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class StationView : GraphicView, IContainerView
    {
        [XmlAttribute]
        public string Name { get; set; }

        [PropertyIgnore, XmlElement("PSDoor")]
        public PSDoorView[] Doors { get; set; }

        [PropertyIgnore, XmlElement("StationButton")]
        public StationButtonView[] Buttons { get; set; }

        private string sectionUp;
        [XmlAttribute]
        public string SectionUp
        {
            get => sectionUp;
            set
            {
                sectionUp = value;
                if (Doors != null)
                {
                    Doors[0].Model.SectionName = value;
                    for (int i = 0; i < 3; i++)
                        Buttons[i].Model.SectionName = value;
                }
            }
        }

        private string sectionDown;
        [XmlAttribute]
        public string SectionDown
        {
            get => sectionDown;
            set 
            { 
                sectionDown = value;
                if (Doors != null && Doors.Length > 1)
                {
                    Doors[1].Model.SectionName = value;
                    for (int i = 3; i < 6; i++)
                        Buttons[i].Model.SectionName = value;
                }
            }
        }

        [XmlIgnore]
        public override Point TopLeft 
        { 
            get => base.TopLeft;
            set
            {
                var offset = value - base.TopLeft;
                if (Doors is not null)
                {
                    foreach (var door in Doors)
                        door.TopLeft += offset;
                }

                if (Buttons is not null)
                {
                    foreach (var button in Buttons)
                        button.TopLeft += offset;
                }

                base.TopLeft = value;
            }
        }

        public override void Render()
        {
            using var dc = Drawing.Open();
            Shapes[0].Render(dc, rectPen);
        }

        public void AddChildren2Canvas(Canvas canvas)
        {
            if (Doors != null)
                foreach (var item in Doors)
                    canvas.Children.Add(item.UI);

            if (Buttons != null)
                foreach (var item in Buttons)
                    canvas.Children.Add(item.UI);

            Canvas.SetZIndex(UI, -1);
        }

        public void AddChildren2Models(CbiModel cbiModel)
        {
            if (Doors is not null)
                foreach (var item in Doors)
                    cbiModel.Devices.Add(item.DeviceInfo);

            if (Buttons is not null)
                foreach (var item in Buttons)
                    cbiModel.Devices.Add(item.DeviceInfo);
        }

        public void RemoveFromGrahics(List<GraphicView> graphics)
        {
            if (Doors is not null)
                foreach (var item in Doors)
                    graphics.Remove(item);

            if (Buttons is not null)
                foreach (var item in Buttons)
                    graphics.Remove(item);
        }

        private static readonly Pen rectPen = new Pen(Brushes.Silver, 3);
    }
}
