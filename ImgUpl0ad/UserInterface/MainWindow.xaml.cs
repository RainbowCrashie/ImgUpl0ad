using System;
using System.Windows;
using System.Windows.Input;
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
                _selectedimage = value;
            }
        }

        private void OpenBydialog(object sender, ExecutedRoutedEventArgs e)
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
    }
}
