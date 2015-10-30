using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace ImgUpl0ad.UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ImageBuff _selectedimage;
        private ImageBuff SelectedImage
        {
            get { return _selectedimage; }
            set
            {
                if (_selectedimage == value) return;
                ButtonUpload.IsEnabled = value != null;
                StretchWhenLarge();
                _selectedimage = value;
            }
        }

        private void OpenByDialog(object sender, ExecutedRoutedEventArgs e)
        {
            var opener = new ImageIo();
            using (opener)
            {
                if (opener.OpenFileWithDialog() == false)
                    return;

                SelectedImage = opener.SelectedImage;
                MainImage.Source = SelectedImage.ImageSouce;
            }
        }

        private void PasteImage(object sernder, ExecutedRoutedEventArgs e)
        {
            if (!Clipboard.ContainsImage())
                return;

            var converter = new BitmapSourceToBitmapImageConverter();
            SelectedImage = new ImageBuff(converter.Convert(Clipboard.GetImage()), "Clipboard");
            MainImage.Source = SelectedImage.ImageSouce;
        }


        private void MainWindow_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                e.Effects = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(DataFormats.StringFormat, true))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }
        private void MainWindow_OnDragDrop(object sender, DragEventArgs e)
        {
            var dropChecker = new ImageIo();
            using (dropChecker)
            {
                if (!dropChecker.CheckDrop(e))
                    return;

                SelectedImage = dropChecker.SelectedImage;
                MainImage.Source = SelectedImage.ImageSouce;
            }
        }

        private void StretchWhenLarge()
        {
            if (SelectedImage == null)
                return;
            if (MainImage.ActualWidth < SelectedImage.ImageSouce.PixelWidth ||
                MainImage.ActualHeight < SelectedImage.ImageSouce.PixelHeight)
            {
                MainImage.Stretch = Stretch.Uniform;
            }
            else
            {
                MainImage.Stretch = Stretch.None;
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            StretchWhenLarge();
        }
    }
}
