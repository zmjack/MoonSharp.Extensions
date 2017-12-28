using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MoonSharp.Extensions
{
    public class Web
    {
        public class ConfigOption
        {
            public const string URL_ENCODED = "application/x-www-form-urlencoded";
            public const string FORM_DATA = "multipart/form-data";
            public const string GET = "get";
            public const string POST = "post";

            public string Enctype = URL_ENCODED;
            public string ContentType = null;
            public string UserAgent = "";
            public bool NoBody = false;
            public string Method = "get";
            public string Encoding = "utf-8";
            public string ProxyAddress = "";
            public string ProxyUsername = "";
            public string ProxyPassword = "";

            public static explicit operator ConfigOption(Dictionary<string, object> dict)
            {
                var instance = new ConfigOption();
                instance.Enctype = Config.Get(dict, "enctype", instance.Enctype);
                instance.ContentType = Config.Get(dict, "content-type", instance.ContentType);
                instance.UserAgent = Config.Get(dict, "user-agent", instance.UserAgent);
                instance.NoBody = Config.Get(dict, "nobody", instance.NoBody);
                instance.Method = Config.Get(dict, "method", instance.Method);
                instance.Encoding = Config.Get(dict, "encoding", instance.Encoding);
                instance.ProxyAddress = Config.Get(dict, "proxy_address", instance.ProxyAddress);
                instance.ProxyUsername = Config.Get(dict, "proxy_username", instance.ProxyUsername);
                instance.ProxyPassword = Config.Get(dict, "proxy_password", instance.ProxyPassword);

                return instance;
            }
        }

        public static string Go(string url, Dictionary<string, object> updata, ConfigOption config)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                byte[] requestBody;

                if (config.Enctype != "multipart/form-data")
                {
                    //Format: application/x-www-form-urlencoded
                    var postDataList = new List<string>();
                    foreach (var data in updata)
                    {
                        postDataList.Add($"{data.Key}={HttpUtility.UrlEncode(data.Value.ToString())}");
                    }
                    requestBody = Encoding.UTF8.GetBytes(string.Join("&", postDataList));
                }
                else
                {
                    //Format: multipart/form-data
                    var formData = new HttpFormData();
                    foreach (var data in updata)
                    {
                        formData.AddData(data.Key, data.Value.ToString());
                    }
                    requestBody = formData;
                }

                if (!config.NoBody)
                    request.ContentLength = requestBody.Length;
                request.ContentType = config.ContentType;
                request.UserAgent = config.UserAgent;
                request.Method = config.Method;
                request.Timeout = -1;
                request.UseDefaultCredentials = true;
                if (!string.IsNullOrEmpty(config.ProxyAddress))
                {
                    request.Proxy = new WebProxy
                    {
                        Address = new Uri(config.ProxyAddress),
                        Credentials = new NetworkCredential
                        {
                            UserName = config.ProxyUsername,
                            Password = config.ProxyPassword,
                        }
                    };
                }

                //Invoke
                if (config.Method == "post" && !config.NoBody)
                    request.GetRequestStream().Write(requestBody, 0, requestBody.Length);

                using (var response = request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(config.Encoding));
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex) { return $"Exception: {ex.Message}"; }
        }

    }
}
