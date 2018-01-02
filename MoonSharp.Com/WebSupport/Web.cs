using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            public const string GET = "GET";
            public const string POST = "POST";

            public string ContentType = URL_ENCODED;
            public string UserAgent = "";
            public string Method = GET;
            public string Encoding = "utf-8";
            public string ProxyAddress = "";
            public string ProxyUsername = "";
            public string ProxyPassword = "";

            public static explicit operator ConfigOption(Dictionary<string, object> dict)
            {
                var instance = new ConfigOption();
                instance.ContentType = Config.Get(dict, "content-type", instance.ContentType);
                instance.UserAgent = Config.Get(dict, "user-agent", instance.UserAgent);
                instance.Method = Config.Get(dict, "method", instance.Method).ToUpper();
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

                switch (config.ContentType)
                {
                    default:
                    case ConfigOption.URL_ENCODED:
                        var postDataList = new List<string>();
                        foreach (var data in updata)
                        {
                            IEnumerable<string> values = new string[0];

                            if (data.Value is Table)
                            {
                                values = (data.Value as Table).Values.AsObjects<string>();
                            }
                            else if (data.Value is Array)
                            {
                                var list = new List<string>();
                                foreach (var value in data.Value as Array)
                                    list.Add(value.ToString());
                                values = list.ToArray();
                            }

                            if (values.Any())
                            {
                                var i = 0;
                                foreach (var value in values)
                                {
                                    postDataList.Add($"{data.Key}[{i++}]={HttpUtility.UrlEncode(value)}");
                                }
                            }
                            else postDataList.Add($"{data.Key}={HttpUtility.UrlEncode(data.Value.ToString())}");
                        }
                        requestBody = Encoding.UTF8.GetBytes(string.Join("&", postDataList));
                        break;

                    case ConfigOption.FORM_DATA:
                        var formData = new HttpFormData();
                        foreach (var data in updata)
                        {
                            formData.AddData(data.Key, data.Value.ToString());
                        }
                        requestBody = formData;
                        break;
                }

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
                if (config.Method == ConfigOption.POST)
                {
                    request.ContentLength = requestBody.Length;
                    request.GetRequestStream().Write(requestBody, 0, requestBody.Length);
                }

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
