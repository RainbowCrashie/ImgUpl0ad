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
            HwndSource HwnDSource;
            // WmNchittest and MOUSEHOOKSTRUCT Mouse Position Codes
            public const int HTERROR = (-2);
            public const int HTTRANSPARENT = (-1);
            public const int HTNOWHERE = 0;
            public const int HTCLIENT = 1;
            public const int HTCAPTION = 2;
            public const int HTSYSMENU = 3;
            public const int HTGROWBOX = 4;
            public const int HTSIZE = HTGROWBOX;
            public const int HTMENU = 5;
            public const int HTHSCROLL = 6;
            public const int HTVSCROLL = 7;
            public const int HTMINBUTTON = 8;
            public const int HTMAXBUTTON = 9;
            public const int HTLEFT = 10;
            public const int HTRIGHT = 11;
            public const int HTTOP = 12;
            public const int HTTOPLEFT = 13;
            public const int HTTOPRIGHT = 14;
            public const int HTBOTTOM = 15;
            public const int HTBOTTOMLEFT = 16;
            public const int HTBOTTOMRIGHT = 17;
            public const int HTBORDER = 18;
            public const int HTREDUCE = HTMINBUTTON;
            public const int HTZOOM = HTMAXBUTTON;
            public const int HTSIZEFIRST = HTLEFT;
            public const int HTSIZELAST = HTBOTTOMRIGHT;
            public const int HTOBJECT = 19;
            public const int HTCLOSE = 20;
            public const int HTHELP = 21;
        #endregion

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            HwnDSource = (HwndSource)PresentationSource.FromVisual(this);
            HwnDSource.AddHook(HwndSourceHook);

            base.OnSourceInitialized(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            HwnDSource.RemoveHook(HwndSourceHook);

            base.OnClosed(e);
        }

        public IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WmNchittest)
            {
                // これ以上処理させない（完全に処理を横取りする）
                handled = true;

                // クライアント座標のマウス位置を取得
                System.Windows.Point ptScreen = new System.Windows.Point(unchecked((short)(long)lParam), unchecked((short)((long)lParam >> 16)));
                System.Windows.Point ptClient = PointFromScreen(ptScreen);


                // リサイズ可能と判断するサイズ
                double bh = SystemParameters.ResizeFrameHorizontalBorderHeight;
                double bw = SystemParameters.ResizeFrameVerticalBorderWidth;
                double captionH = SystemParameters.CaptionHeight;


                // 四隅の斜め方向リサイズが優先
                if (new Rect(0, 0, bw, bh).Contains(ptClient)) return new IntPtr(HTTOPLEFT);
                if (new Rect(Width - bw, 0, bw, bh).Contains(ptClient)) return new IntPtr(HTTOPRIGHT);
                if (new Rect(0, Height - bh, bw, bh).Contains(ptClient)) return new IntPtr(HTBOTTOMLEFT);
                if (new Rect(Width - bw, Height - bh, bw, bh).Contains(ptClient)) return new IntPtr(HTBOTTOMRIGHT);


                // 四辺の直交方向リサイズ
                if (new Rect(0, 0, Width, bw).Contains(ptClient)) return new IntPtr(HTTOP);
                if (new Rect(0, 0, bw, Height).Contains(ptClient)) return new IntPtr(HTLEFT);
                if (new Rect(Width - bw, 0, bw, Height).Contains(ptClient)) return new IntPtr(HTRIGHT);
                if (new Rect(0, Height - bh, Width, bh).Contains(ptClient)) return new IntPtr(HTBOTTOM);


                // ドラッグ移動可能な領域を指定
                if (new Rect(0, 0, Width, captionH).Contains(ptClient)) return new IntPtr(HTCAPTION);


                // 上記以外はクライアント領域と判断
                return new IntPtr(HTCLIENT);
            }

            return IntPtr.Zero;
        }
        #endregion

        private void ButtonCapture_OnClick(object sender, RoutedEventArgs e)
        {

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
