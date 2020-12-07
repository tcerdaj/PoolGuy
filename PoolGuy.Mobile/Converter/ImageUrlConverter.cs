using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using Xamarin.Forms;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;

namespace PoolGuy.Mobile.Converter
{
    public class ImageUrlConverter : IValueConverter
    {
        static WebClient _client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = value as string;

                if (string.IsNullOrEmpty(val) || value == null)
                {
                    return "addphotoblack.png";
                }

                if (val.StartsWith("http"))
                {
                    //ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //var imageArray = _client.DownloadData(val);

                    //return ImageSource.FromStream(()=> { return new MemoryStream(imageArray); });
                    return new UriImageSource
                    {
                        Uri = new Uri(val),
                        CachingEnabled = true,
                        CacheValidity = new TimeSpan(183, 0, 0, 0)
                    };
                }

                return new FileImageSource
                {
                    File = val
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "addphotoblack.png";
            }
        }

        private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HttpMessageHandlerDelegate : DelegatingHandler
    {
        public HttpMessageHandlerDelegate()
            :base(new HttpClientHandler())
        {

        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            return base.SendAsync(request, cancellationToken);
        }
    }
}
