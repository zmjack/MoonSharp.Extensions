using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MoonSharp.Extensions
{
    public class WebSupport : LuaSupport
    {
        [LuaFunction("en-US", "Visit web。f(URL, updata, config[" +
            "'enctype', 'content-type', 'user-agent', 'nobody', 'method', 'encoding', " +
            "'proxy_address', 'proxy_username', 'proxy_password'])")]
        [LuaFunction("zh-CN", "访问web资源。f(URL, 传送数据, 客户端配置[" +
            "'enctype', 'content-type', 'user-agent', 'nobody', 'method', 'encoding', " +
            "'proxy_address', 'proxy_username', 'proxy_password'])")]
        public static string Go(string url, Dictionary<string, object> updata, Dictionary<string, object> config)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                byte[] requestBody;

                var enctype = Config.Get(config, "enctype", "application/x-www-form-urlencoded");
                var contentType = Config.Get(config, "content-type", "application/x-www-form-urlencoded");
                var userAgent = Config.Get(config, "user-agent", "");
                var nobody = Config.Get(config, "nobody", @default: false);
                var method = Config.Get(config, "method", "get");
                var encoding = Encoding.GetEncoding(Config.Get(config, "encoding", "utf-8"));
                var proxy_address = Config.Get(config, "proxy_address", "");
                var proxy_username = Config.Get(config, "proxy_username", "");
                var proxy_password = Config.Get(config, "proxy_password", "");

                if (enctype != "multipart/form-data")
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

                if (!nobody)
                    request.ContentLength = requestBody.Length;
                request.ContentType = contentType;
                request.UserAgent = userAgent;
                request.Method = method;
                request.Timeout = -1;
                request.UseDefaultCredentials = true;
                if (!string.IsNullOrEmpty(proxy_address))
                {
                    request.Proxy = new WebProxy
                    {
                        Address = new Uri(proxy_address),
                        Credentials = new NetworkCredential
                        {
                            UserName = proxy_username,
                            Password = proxy_password,
                        }
                    };
                }

                //Invoke
                if (method == "post" && !nobody)
                    request.GetRequestStream().Write(requestBody, 0, requestBody.Length);

                using (var response = request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream(), encoding);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex) { return $"Exception: {ex.Message}"; }
        }

    }
}
