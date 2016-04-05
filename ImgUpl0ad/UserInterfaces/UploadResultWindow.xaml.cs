using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImgUpl0ad.UserInterfaces
{
    /// <summary>
    /// Interaction logic for UploadResultWindow.xaml
    /// </summary>
    public partial class UploadResultWindow
    {
        public UploadResultWindow(ImageBuff source)
        {
            InitializeComponent();
            Upload(source);
        }

        private async void Upload(ImageBuff img)
        {
            var imgur = new Imgur();
            var link = await imgur.UploadAsync(img.ImageSource);

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
    }
}
