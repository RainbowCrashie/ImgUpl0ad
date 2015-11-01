using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace ImgUpl0ad.UserInterfaces
{
    /// <summary>
    /// Interaction logic for UploadResultWindow.xaml
    /// </summary>
    public partial class UploadResultWindow
    {
        private ImageBuff Source { get; }
        public UploadResultWindow(ImageBuff source)
        {
            InitializeComponent();
            Source = source;
        }

        private async void Upload()
        {
            var imgur = new Imgur();
            var link = await imgur.UploadAsync(Source.ImageSouce);

            ProcessingIndicator.IsActive = false;
            labelProgress.Content = "Uploaded Successful";
            tbURL.Text = link;
            tbURL.Visibility = Visibility.Visible;
            ButtonOk.Visibility = Visibility.Visible;
            imageUploaded.Source = new BitmapImage(new Uri(link));
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UploadResultWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Upload();
        }
    }
}
