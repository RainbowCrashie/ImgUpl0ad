using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ImgUpl0ad.UserInterfaces
{
    /// <summary>
    /// Interaction logic for CaptureWindow.xaml
    /// </summary>
    public partial class CaptureWindow
    {
        public CaptureWindow()
        {
            InitializeComponent();
        }

        #region ReSiZe
        #region "Constants"
            private const int WmNchittest = 0x0084;
            HwndSource _hwnDSource;
            // WmNchittest and MOUSEHOOKSTRUCT Mouse Position Codes
            public const int Hterror = (-2);
            public const int Httransparent = (-1);
            public const int Htnowhere = 0;
            public const int Htclient = 1;
            public const int Htcaption = 2;
            public const int Htsysmenu = 3;
            public const int Htgrowbox = 4;
            public const int Htsize = Htgrowbox;
            public const int Htmenu = 5;
            public const int Hthscroll = 6;
            public const int Htvscroll = 7;
            public const int Htminbutton = 8;
            public const int Htmaxbutton = 9;
            public const int Htleft = 10;
            public const int Htright = 11;
            public const int Httop = 12;
            public const int Httopleft = 13;
            public const int Httopright = 14;
            public const int Htbottom = 15;
            public const int Htbottomleft = 16;
            public const int Htbottomright = 17;
            public const int Htborder = 18;
            public const int Htreduce = Htminbutton;
            public const int Htzoom = Htmaxbutton;
            public const int Htsizefirst = Htleft;
            public const int Htsizelast = Htbottomright;
            public const int Htobject = 19;
            public const int Htclose = 20;
            public const int Hthelp = 21;
        #endregion

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            _hwnDSource = (HwndSource)PresentationSource.FromVisual(this);
            _hwnDSource.AddHook(HwndSourceHook);

            base.OnSourceInitialized(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _hwnDSource.RemoveHook(HwndSourceHook);

            base.OnClosed(e);
        }

        public IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WmNchittest)
            {
                // これ以上処理させない（完全に処理を横取りする）
                handled = true;

                // クライアント座標のマウス位置を取得
                var ptScreen = new Point(unchecked((short)(long)lParam), unchecked((short)((long)lParam >> 16)));
                var ptClient = PointFromScreen(ptScreen);


                // リサイズ可能と判断するサイズ
                double bh = SystemParameters.ResizeFrameHorizontalBorderHeight;
                double bw = SystemParameters.ResizeFrameVerticalBorderWidth;
                double captionH = SystemParameters.CaptionHeight;


                // 四隅の斜め方向リサイズが優先
                if (new Rect(0, 0, bw, bh).Contains(ptClient)) return new IntPtr(Httopleft);
                if (new Rect(Width - bw, 0, bw, bh).Contains(ptClient)) return new IntPtr(Httopright);
                if (new Rect(0, Height - bh, bw, bh).Contains(ptClient)) return new IntPtr(Htbottomleft);
                if (new Rect(Width - bw, Height - bh, bw, bh).Contains(ptClient)) return new IntPtr(Htbottomright);


                // 四辺の直交方向リサイズ
                if (new Rect(0, 0, Width, bw).Contains(ptClient)) return new IntPtr(Httop);
                if (new Rect(0, 0, bw, Height).Contains(ptClient)) return new IntPtr(Htleft);
                if (new Rect(Width - bw, 0, bw, Height).Contains(ptClient)) return new IntPtr(Htright);
                if (new Rect(0, Height - bh, Width, bh).Contains(ptClient)) return new IntPtr(Htbottom);


                // ドラッグ移動可能な領域を指定
                if (new Rect(0, 0, Width, captionH).Contains(ptClient)) return new IntPtr(Htcaption);


                // 上記以外はクライアント領域と判断
                return new IntPtr(Htclient);
            }

            return IntPtr.Zero;
        }
        #endregion

        public ImageBuff CapturedImage { get; set; }

        private void ButtonCapture_OnClick(object sender, RoutedEventArgs e)
        {
            Opacity = 0;

            var location = GetThisLocation();
            var size = GetThisSize();
            
            var capture = new Screencap();
            CapturedImage = capture.Capture(location, size);

            Opacity = 1;
            DialogResult = true;
        }

        private void GridCaptureArea_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShowSizeLocation();
        }

        private void CaptureWindow_OnLocationChanged(object sender, EventArgs e)
        {
            ShowSizeLocation();
        }

        private Point GetThisLocation()
        {
            Point location = GridCaptureArea.PointToScreen(new Point(0.0d, 0.0d));
            return location;
        }

        private Point GetThisSize()
        {
            Point size = new Point(GridCaptureArea.ActualWidth, GridCaptureArea.ActualHeight);
            return size;
        }

        private void ShowSizeLocation()
        {
            Point pt = GetThisLocation();
            Point sz = GetThisSize();

            LabelLocSize.Content = ($"({pt.X},{pt.Y})({sz.X},{sz.Y})");
        }
    }
}
