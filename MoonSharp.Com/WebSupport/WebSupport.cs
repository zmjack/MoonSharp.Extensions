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
                return Web.Go(url, updata, (Web.ConfigOption)config);
            }
            catch (Exception ex) { return $"Exception: {ex.Message}"; }
        }

    }
}
