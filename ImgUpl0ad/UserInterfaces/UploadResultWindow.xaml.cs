using System;
using System.Windows;

using MahApps.Metro.Controls;

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

        private async void Upload(ImageBuff source)
        {
            
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
