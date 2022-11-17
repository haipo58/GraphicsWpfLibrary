using RailwaySignalsModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GraphicsWpfLibrary
{
    public class SectionView : CommandDevice, ISection
    {
        [PropertyIgnore, XmlIgnore]
        public override DeviceModel DeviceInfo => Model;

        [PropertyShow]
        public SectionModel Model { get; set; }

        [PropertyIgnore, XmlIgnore]
        public SectionStatus Status { get; } = new();

        [PropertyIgnore, XmlIgnore]
        public SectionCore Node { get; set; }

        #region render

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawSectionLines(dc, GetSectionPen(Status), Shapes, Status.IsBlocked);

            if (InsulationsVisable)
                DrawInsulations(dc);

            RenderName(dc);
        }

        protected void DrawInsulations(DrawingContext dc)
        {
            Point left = ((Line)Shapes[0]).Pt0;
            Point right = ((Line)Shapes[^1]).Pt1;

            DrawInsuLine(dc, left, 0);
            DrawInsuLine(dc, right, 0);
        }

        public static void DrawInsuLine(DrawingContext dc, Point point, int rotateAngle = 0)
        {

            if (rotateAngle == 0)
            {
                dc.DrawLine(InsulationPen, new Point(point.X, point.Y - InsulationHeight), new Point(point.X, point.Y + InsulationHeight));
                return;
            }

            double radian = rotateAngle / 180.0 * Math.PI;
            double x = InsulationHeight * Math.Cos(radian);
            double y = InsulationHeight * Math.Sin(radian);
            dc.DrawLine(InsulationPen, new Point(point.X + x, point.Y - y), new Point(point.X - x, point.Y + y));
        }

        public static void DrawSectionLines(DrawingContext dc, Pen pen, IEnumerable<Shape> shapes
            , bool isBlocked, bool isSingleLocked = false)
        {
            foreach (var line in shapes)
            {
                if (isBlocked)
                    line.Render(dc, BlockPen);

                if (isSingleLocked)
                    line.Render(dc, SingleLockPen);

                line.Render(dc, pen);
            }
        }

        #endregion render

        static public bool InsulationsVisable { get; set; } = true;
        public static int InsulationHeight { get; set; } = 6;
        #region Pens
        public static Pen GetSectionPen(SectionStatus status) =>
            status.IsOccupied ? OccupyPen
                : status.IsLocked ? LockPen
                : status.IsProtected ? ProtectPen
                : ClearPen;

        public static void SetPenThickness(int value)
        {
            OccupyPen = new Pen(Brushes.Red, value);
            LockPen = new Pen(Brushes.White, value);
            BlockPen = new Pen(Brushes.Red, value + 6);
            SingleLockPen = new Pen(Brushes.Silver, value + 6);
            ProtectPen = new Pen(Brushes.Yellow, value);
            ClearPen = new Pen(Brushes.Cyan, value);
        }

        public static Pen OccupyPen { get; private set; } = new Pen(Brushes.Red, 3);
        public static Pen LockPen { get; private set; } = new Pen(Brushes.White, 3);
        public static Pen BlockPen { get; private set; } = new Pen(Brushes.Red, 7);
        public static Pen SingleLockPen { get; private set; } = new Pen(Brushes.Silver, 7);
        public static Pen ProtectPen { get; private set; } = new Pen(Brushes.Yellow, 3);
        public static Pen ClearPen { get; set; } = new Pen(Brushes.Cyan, 3);
        public static Pen InsulationPen { get; set; } = new Pen(Brushes.White, 3);
        #endregion Pens
    }
}