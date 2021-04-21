using RailwaySignalsModels;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class RailswitchView : CommandDevice, IFlashItem, IRailswitch
    {
        private bool flashFlag;

        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public RailswitchModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public RailswitchStatus Status { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public SectionStatus SectionStatus { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public override bool IsFlashing => IsSelected
            || Status.Position == RailswitchPosition.None
            || Status.Position == RailswitchPosition.Fault;

        [PropertyIgnore, XmlAttribute]
        public bool IsLeftOpen { get; set; }

        [PropertyIgnore, XmlAttribute]
        public bool IsUpOpen { get; set; }

        [PropertyIgnore, XmlIgnore]
        public RailswitchNode Node { get; set; }

        #region shapes
        [PropertyIgnore]
        public List<Line> SectionLines { get; set; }

        [PropertyIgnore]
        public List<Line> NormalLines { get; set; }

        [PropertyIgnore]
        public List<Line> ReverseLines { get; set; }

        [PropertyIgnore]
        public Line NormalNail { get; set; }

        [PropertyIgnore]
        public Line ReverseNail { get; set; }
        #endregion

        #region render
        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            RenderShapes(dc, SectionView.GetSectionPen(SectionStatus), SectionStatus.IsBlocked);
            RenderName(dc);
        }

        public override void RenderName(DrawingContext dc)
        {
            Brush foreBrush = Brushes.Silver;
            if (Status.Position == RailswitchPosition.Fault)
                foreBrush = Brushes.White;
            if (NameFlashFlag && IsSelected)
                foreBrush = Brushes.DarkSlateGray;

            RenderName(dc, foreBrush);
        }

        private void RenderShapes(DrawingContext dc, Pen sectionPen, bool isBlocked)
        {
            switch (Status.Position)
            {
                case RailswitchPosition.None:
                    DrawSectionLines(dc, sectionPen, sectionPen, sectionPen, isBlocked);
                    DrawFlashNails(dc, SectionView.ClearPen, isBlocked);
                    break;
                case RailswitchPosition.Fault:
                    DrawSectionLines(dc, sectionPen, sectionPen, sectionPen, isBlocked);
                    DrawFlashNails(dc, SectionView.OccupyPen, isBlocked);
                    break;
                case RailswitchPosition.Normal:
                    DrawSectionLines(dc, sectionPen, sectionPen, SectionView.ClearPen, isBlocked);
                    DrawNail(dc, sectionPen, NormalNail, isBlocked);
                    break;
                case RailswitchPosition.Reverse:
                    DrawSectionLines(dc, sectionPen, SectionView.ClearPen, sectionPen, isBlocked);
                    DrawNail(dc, sectionPen, ReverseNail, isBlocked);
                    break;
                default:
                    break;
            }
        }

        private void DrawFlashNails(DrawingContext dc, Pen nailPen, bool isBlocked)
        {
            if (flashFlag)
            {
                DrawNail(dc, nailPen, NormalNail, isBlocked);
                DrawNail(dc, nailPen, ReverseNail, isBlocked);
            }
        }

        private void DrawSectionLines(DrawingContext dc, Pen sectionPen, Pen normalPen, Pen reversePen, bool isBlocked)
        {
            SectionView.DrawSectionLines(dc, sectionPen, SectionLines, isBlocked);
            SectionView.DrawSectionLines(dc, normalPen, NormalLines, isBlocked);
            SectionView.DrawSectionLines(dc, reversePen, ReverseLines, isBlocked);
        }

        private static void DrawNail(DrawingContext dc, Pen pen, Line nail, bool isBlocked)
        {
            if (isBlocked)
                dc.DrawLine(SectionView.BlockPen, nail.Pt0, nail.Pt1);

            dc.DrawLine(pen, nail.Pt0, nail.Pt1);
        }

        #endregion

        public override void OnFlashTimer()
        {
            base.OnFlashTimer();
            flashFlag = !flashFlag;
        }

        public void AddToShapes()
        {
            Shapes = new() { NormalNail, ReverseNail };
            Shapes.AddRange(SectionLines);
            Shapes.AddRange(NormalLines);
            Shapes.AddRange(ReverseLines);
        }
    }
}