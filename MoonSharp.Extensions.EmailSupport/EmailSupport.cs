using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MoonSharp.Extensions
{
    public class EmailSupport : LuaSupport
    {
        [LuaFunction("en-US", "Send Email. f()")]
        [LuaFunction("zh-CN", "发送邮件。f(客户端配置[host, port, username, password, timeout, ssl]," +
            "邮件[from, to(;), cc(;), bcc(;), subject, subjectencoding, body, bodyencoding, isbodyhtml, priority(0n,1l,2h)])")]
        public static string Send(Dictionary<string, object> client, Dictionary<string, object> email)
        {
            try
            {
                //Client
                var host = Config.Get(client, "host", "");
                var port = Config.Get(client, "port", 25);
                var username = Config.Get(client, "username", "");
                var password = Config.Get(client, "password", "");
                var timeout = Config.Get(client, "timeout", 100000);
                var ssl = Config.Get(client, "ssl", false);

                var smtpClient = new SmtpClient()
                {
                    Host = host,
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = ssl,
                    Timeout = timeout,
                };

                //Email
                var from = Config.Get(email, "from", "");
                var to = Config.Get(email, "to", "").Split(';').Select(a => a.Trim())
                    .Where(a => !string.IsNullOrEmpty(a));
                var cc = (Config.Get(email, "cc", null)?.Split(';').Select(a => a.Trim()) ?? new string[0])
                    .Where(a => !string.IsNullOrEmpty(a));
                var bcc = Config.Get(email, "bcc", null)?.Split(';').Select(a => a.Trim()) ?? new string[0]
                    .Where(a => !string.IsNullOrEmpty(a));
                var subject = Config.Get(email, "subject", "");
                var subjectEncoding = Encoding.GetEncoding(
                    Config.Get(email, "subjectencoding", "utf-8"));
                var body = Config.Get(email, "body", "");
                var bodyEncoding = Encoding.GetEncoding(
                    Config.Get(email, "bodyEncoding", "utf-8"));
                var isbodyhtml = Config.Get(email, "isbodyhtml", true);
                var priority = Config.Get(email, "priority", MailPriority.Normal);

                var mail = new MailMessage()
                {
                    From = new MailAddress(from),
                    Subject = subject,
                    SubjectEncoding = subjectEncoding,
                    Body = body,
                    BodyEncoding = bodyEncoding,
                    IsBodyHtml = isbodyhtml,
                    Priority = priority,
                };

                foreach (var address in to)
                    mail.To.Add(address);
                foreach (var address in cc)
                    mail.CC.Add(address);
                foreach (var address in bcc)
                    mail.Bcc.Add(address);

                smtpClient.Send(mail);

                return "Success";
            }
            catch (Exception ex) { return $"Exception: {ex.Message}"; }
        }

    }
}
