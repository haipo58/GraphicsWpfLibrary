using RailwaySignalsModels;
using System.Collections.Generic;
using System.Windows;
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

        [PropertyIgnore, XmlIgnore]
        public RailswitchCore Node { get; set; }

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

        static public int Angle { get; set; } = 45;
        #endregion

        #region render
        public override void Render(DrawingContext dc)
        {
            base.Render(dc);

            RenderShapes(dc, SectionView.GetSectionPen(SectionStatus),
                SectionStatus.IsBlocked,
                Status.IsSingleLocked);

            RenderName(dc);
            DrawInsuLines(dc);
        }

        public void DrawInsuLines(DrawingContext dc)
        {
            if (SectionView.InsulationsVisable && !string.IsNullOrEmpty(Model.DoubleName))
                SectionView.DrawInsuLine(dc, ReverseLines[0].Pt1,
                    Model.IsLeftOpen == Model.IsUpOpen ? Angle : -Angle);
        }

        public override void RenderName(DrawingContext dc)
        {
            if (!IsNameVisable)
                return;

            Brush foreBrush = Brushes.Silver;

            if (Status.Position == RailswitchPosition.Fault)
                foreBrush = Brushes.White;
            if (NameFlashFlag && IsSelected)
                foreBrush = Brushes.DarkSlateGray;

            RenderName(dc, foreBrush);
        }

        public virtual void RenderShapes(DrawingContext dc, Pen sectionPen, bool isBlocked, bool isSingleLocked)
        {
            switch (Status.Position)
            {
                case RailswitchPosition.None:
                    DrawSectionLines(dc, sectionPen, sectionPen, sectionPen, isBlocked, isSingleLocked);
                    DrawFlashNails(dc, SectionView.ClearPen, isBlocked, isSingleLocked);
                    break;
                case RailswitchPosition.Fault:
                    DrawSectionLines(dc, sectionPen, sectionPen, sectionPen, isBlocked, isSingleLocked);
                    DrawFlashNails(dc, SectionView.OccupyPen, isBlocked, isSingleLocked);
                    break;
                case RailswitchPosition.Normal:
                    DrawSectionLines(dc, sectionPen, sectionPen, SectionView.ClearPen, isBlocked, isSingleLocked);
                    DrawNail(dc, sectionPen, NormalNail, isBlocked, isSingleLocked);
                    break;
                case RailswitchPosition.Reverse:
                    DrawSectionLines(dc, sectionPen, SectionView.ClearPen, sectionPen, isBlocked, isSingleLocked);
                    DrawNail(dc, sectionPen, ReverseNail, isBlocked, isSingleLocked);
                    break;
                default:
                    break;
            }
        }

        protected void DrawFlashNails(DrawingContext dc, Pen nailPen, bool isBlocked, bool isSingleLocked)
        {
            if (flashFlag)
            {
                DrawNail(dc, nailPen, NormalNail, isBlocked, isSingleLocked);
                DrawNail(dc, nailPen, ReverseNail, isBlocked, isSingleLocked);
            }
        }

        protected void DrawSectionLines(DrawingContext dc, Pen sectionPen, Pen normalPen, Pen reversePen
            , bool isBlocked, bool isSingleLocked)
        {
            SectionView.DrawSectionLines(dc, sectionPen, SectionLines, isBlocked, isSingleLocked);
            SectionView.DrawSectionLines(dc, normalPen, NormalLines, isBlocked, isSingleLocked);
            SectionView.DrawSectionLines(dc, reversePen, ReverseLines, isBlocked, isSingleLocked);
        }

        protected static void DrawNail(DrawingContext dc, Pen pen, Line nail
            , bool isBlocked, bool isSingleLocked)
        {
            if (isBlocked)
                nail.Render(dc, SectionView.BlockPen);

            if (isSingleLocked)
                nail.Render(dc, SectionView.SingleLockPen);

            nail.Render(dc, pen);
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

        public override void AddShape(Shape shape)
        {
            if (shape is Line connectLine)
            {
                if (AddConnectLine(connectLine, SectionLines))
                    return;

                if (AddConnectLine(connectLine, ReverseLines))
                    return;

                if (AddConnectLine(connectLine, NormalLines))
                    return;
            }
        }

        private static bool AddConnectLine(Line connectLine, List<Line> lines)
        {
            Point temp;
            foreach (var line in lines)
                if (connectLine.InRange(line.Pt0, ref temp, 1)
                    || connectLine.InRange(line.Pt1, ref temp, 1))
                {
                    lines.Add(connectLine);
                    return true;
                }

            return false;
        }
    }
}