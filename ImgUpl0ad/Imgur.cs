﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Media.Imaging;

namespace ImgUpl0ad
{
    public class Imgur : IDisposable
    {
        private HttpClient _client;
        private string ClientID { get; } = "d404730f4975f11";

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

        public async Task<string> UploadAsync(BitmapImage image)
        {
            var converter = new BitmapImageToByteArrayConverter();
            var content = new ByteArrayContent(converter.Convert(image));

            var response = await _client.PostAsync("image", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var model = new JavaScriptSerializer().Deserialize<dynamic>(responseContent);
            var link = model["data"]["link"];

            return link;
        }

        private class BitmapImageToByteArrayConverter
        {
            public byte[] Convert(BitmapImage source)
            {
                BitmapEncoder encoder = null;

                encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    return ms.ToArray();
                }
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
