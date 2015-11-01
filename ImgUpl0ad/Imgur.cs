using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Media.Imaging;

namespace ImgUpl0ad
{
    public class Imgur : IDisposable
    {
        private HttpClient _client;
        private string ClientID { get; } = "";

        public Imgur()
        {
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.imgur.com/3/");
            _client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {ClientID}");
        }

        public async Task<string> UploadAsync(BitmapImage imageSource)
        {
            var converter = new BitmapImageToByteArrayConverter();
            var content = new ByteArrayContent(converter.Convert(imageSource));

            var response = await _client.PostAsync("image", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var model = new JavaScriptSerializer().Deserialize<dynamic>(responseContent);
            var link = model["data"]["link"];

            return link;
        }
        
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
