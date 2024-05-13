using System.Windows;
using System.Windows.Input;

namespace ScreenScale;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class ScreenScaleWindow : Window {
   public ScreenScaleWindow () {
      InitializeComponent ();
      MouseDown += (sender, e) => {
         if (e.ChangedButton == MouseButton.Left)
            DragMove ();
      };
      MouseDoubleClick += (sender, e) => {
         mScaleCanvas.ToggleOrientation ();
      };
      MouseMove += (sender, e) => {
         var pt = e.GetPosition (mScaleCanvas);
         mScaleCanvas.ShowMousePointLine (pt.X, pt.Y);
      };
      KeyDown += (sender, e) => {
         if (e.Key == Key.Escape) Application.Current.Shutdown ();
      };
   }
}
