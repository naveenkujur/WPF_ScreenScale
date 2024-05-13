using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScreenScale;

class ScaleCanvas : Canvas {
   #region Constructors
   public ScaleCanvas () {
      Width = ScaleLength; Height = ScaleWidth;
      SnapsToDevicePixels = true;
   }
   #endregion

   #region Methods
   /// <summary>Switch scale orientation b/w vertical and horizontal</summary>
   public void ToggleOrientation () {
      mIsVertical = !mIsVertical;
      Width = mIsVertical ? ScaleWidth : ScaleLength;
      Height = mIsVertical ? ScaleLength : ScaleWidth;
   }

   public void ShowMousePointLine (double mouseX, double mouseY) {
      mCurrentMouseLine = mIsVertical ? mouseY : mouseX;
      InvalidateVisual ();
   }
   #endregion

   #region Canvas overrides
   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);
      var pen = new Pen (Brushes.DarkBlue, 1); pen.Freeze ();
      if (mIsVertical) {
         Matrix m = new Matrix (); m.Rotate (90);
         m.Translate (ScaleWidth, 0);
         dc.PushTransform (new MatrixTransform (m));
      }

      RenderInchScaleMarkings (dc);
      // Show the mouse position line
      dc.DrawLine (new Pen (Brushes.Gray, 1), new Point (mCurrentMouseLine, 0), new Point (mCurrentMouseLine, ScaleWidth));

      if (mIsVertical)
         dc.Pop (); // Pops the transform
   }
   #endregion

   #region Implementation
   void RenderInchScaleMarkings (DrawingContext dc) {
      int tickSpansPerInch = 20; // 20 tick spans, per inch
      int totalTicks = MaxScaleMeasure * tickSpansPerInch + 1;
      double tickSpan = (double)InchPx / tickSpansPerInch;

      double tickLength = 0.45 * InchPx;
      double tickLength2 = 0.35 * InchPx;
      double tickLength3 = 0.25 * InchPx;
      double tickLength4 = 0.1 * InchPx;

      var pen = new Pen (Brushes.Blue, 1); pen.Freeze ();
      var pen2 = new Pen (Brushes.DarkBlue, 1); pen2.Freeze ();

      for (int i = 0; i < totalTicks; i++) {
         double x = ScaleMargin + i * tickSpan;
         if ((i % 20) == 0) {
            dc.DrawLine (pen, new Point (x, 0), new Point (x, tickLength));
            dc.DrawLine (pen, new Point (x, ScaleWidth - tickLength), new Point (x, ScaleWidth));
         } else if ((i % 10) == 0) {
            dc.DrawLine (pen, new Point (x, 0), new Point (x, tickLength2));
            dc.DrawLine (pen, new Point (x, ScaleWidth - tickLength2), new Point (x, ScaleWidth));
         } else if ((i % 2) == 0) {
            dc.DrawLine (pen, new Point (x, 0), new Point (x, tickLength3));
            dc.DrawLine (pen, new Point (x, ScaleWidth - tickLength3), new Point (x, ScaleWidth));
         } else {
            dc.DrawLine (pen2, new Point (x, 0), new Point (x, tickLength4));
            dc.DrawLine (pen2, new Point (x, ScaleWidth - tickLength4), new Point (x, ScaleWidth));
         }
      }
   }
   #endregion

   #region Consts
   const int InchPx = 96; // WPF definition of an inch. [Inch is 96 logical pixels long]
   const int MaxScaleMeasure = 10; // Maximum measurable length, in measurable inches

   const double ScaleMargin = 0.05 * InchPx; // 0.05inch margin
   const double ScaleLength = MaxScaleMeasure * InchPx + 2 * ScaleMargin; // 10inch + 0.1inch margins
   const double ScaleWidth = 1.2 * InchPx;
   #endregion

   #region Private
   bool mIsVertical = false;
   double mCurrentMouseLine = 0;
   #endregion
}
