﻿using System;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace ImgUpl0ad.UserInterface
{
    public class ImageIo : IDisposable
    {
        private string[] CompatibleExtentions { get; } = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        public ImageBuff SelectedImage { get; set; }

        private bool CheckExtentionCompatible(string filePath)
        {
            return CompatibleExtentions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase);
        }

        public bool OpenFileWithDialog()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.jpg;*.png;*.gif;*.bmp;|*.jpg;*.png;*.gif;*.bmp;";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == false)
                return false;

            SelectedImage = new ImageBuff(new BitmapImage(new Uri(openFileDialog.FileName)), openFileDialog.FileName);
            return true;
        }

        #region "destractor"
        private bool _disposed = false;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ImageIo()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
               //Dispose everyrhing
               SelectedImage = null;
            }
            _disposed = true;
        }
#endregion
    }

    public class BitmapSourceToBitmapImageConverter
    {
        public BitmapImage Convert(BitmapSource bitmap)
        {
            var encoder = new JpegBitmapEncoder();
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