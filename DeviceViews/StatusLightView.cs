using RailwaySignalsModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class StatusLightView : DeviceView, IFlashItem
    {
        [PropertyShow]
        public override DeviceModel DeviceInfo { get; set; } = new DeviceModel();

        [PropertyIgnore, XmlIgnore]
        public StatusLightStatus Status { get; set; } = new();

        public override void Render()
        {
            using var dc = Drawing.Open();

            var brush = Status.IsOn || (_isFlashing && !_flashFlag) ? ForeBrush : Brushes.Gray;

            Shapes[0].Render(dc, null, brush);
            RenderName(dc, Brushes.White);
        }

        public void OnFlashTimer() => _flashFlag = !_flashFlag;

        [XmlIgnore]
        public SolidColorBrush ForeBrush { get; set; } = Brushes.Green;

        private bool _isFlashing;
        private bool _flashFlag;
        public bool IsFlashing
        {
            get
            {
                if (_isFlashing != Status.IsWarnning && !Status.IsWarnning)
                {
                    _flashFlag = false;
                    UI.InvalidateVisual();
                }

                return _isFlashing = Status.IsWarnning;
            }
        }
    }
}
