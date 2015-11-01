using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ImgUpl0ad
{
    public class DragDropChecker
    {
        private string[] CompatibleExtentions { get; } = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        private bool CheckExtentionCompatible(string filePath)
        {
            return CompatibleExtentions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase);
        }

        public ImageBuff CheckDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] bunchOfFiles = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (bunchOfFiles.Length > 1)
                    return null; //No Multiple Files!!!!

                if (!CheckExtentionCompatible(bunchOfFiles[0]))
                    return null;
                
                return new ImageBuff(new BitmapImage(new Uri(bunchOfFiles[0])), bunchOfFiles[0]);
            }
            if (e.Data.GetDataPresent(DataFormats.StringFormat, true)) //for dragdops from web browsers
            {
                var someString = (string)e.Data.GetData(DataFormats.StringFormat);

                if (!System.Text.RegularExpressions.Regex.IsMatch(someString,
                      @"https?://[\w/:%#\$&\?\(\)~\.=\+\-]+",
                      System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    return null; //Is it a URL?

                if (!CheckExtentionCompatible(someString))
                    return null;

                return new ImageBuff(new BitmapImage(new Uri(someString)), someString);
            }

            return null;
        }
    }

    public class BitmapSourceToBitmapImageConverter
    {
        public BitmapImage Convert(BitmapSource bitmap)
        {
            //BitmapEncoder encoder = null;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var memoryStream = new MemoryStream();
            encoder.Save(memoryStream);

            var bitmapImage = new BitmapImage();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.None;

            try
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
            }
            finally
            {
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                memoryStream.Close();
            }

            return bitmapImage;
        }
    }

    public class Screencap
    {
        public ImageBuff Capture(System.Windows.Point location, System.Windows.Point size)
        {
            using (var area = new Bitmap(
                (int)size.X,
                (int)size.Y,
                PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(area))
                {
                    bmpGraphics.CopyFromScreen((int)location.X, (int)location.Y, 0, 0, area.Size, CopyPixelOperation.SourceCopy);
                    var source = Imaging.CreateBitmapSourceFromHBitmap(
                                 area.GetHbitmap(),
                                 IntPtr.Zero,
                                 Int32Rect.Empty,
                                 BitmapSizeOptions.FromEmptyOptions());

                    var converter = new BitmapSourceToBitmapImageConverter();
                    return new ImageBuff(converter.Convert(source), "Capture");
                }
            }
        }
    }

    public class ImageBuff
    {
        public ImageBuff(BitmapImage imageSouce, string detail)
        {
            ImageSouce = imageSouce;
            Detail = detail;
        }

        public ImageBuff()
        {
        }

        public BitmapImage ImageSouce { get; set; }
        public string Detail { get; set; }
    }
}