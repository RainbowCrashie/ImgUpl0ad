using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Microsoft.Win32;

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
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.jpg;*.png;*.gif;*.bmp;|*.jpg;*.png;*.gif;*.bmp;";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == false)
                return;

            SelectedImage = new ImageBuff(new BitmapImage(new Uri(openFileDialog.FileName)), openFileDialog.FileName);
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
            var dropChecker = new DragDropChecker();
            var imgbuff = dropChecker.CheckDrop(e);
            if (imgbuff == null)
                return;

            SelectedImage = imgbuff;
            MainImage.Source = SelectedImage.ImageSouce;
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
