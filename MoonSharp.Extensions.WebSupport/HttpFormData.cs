using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MoonSharp.Extensions
{
    public class HttpFormData
    {
        /// <summary>
        /// [name] = (fileName, bytes)
        /// </summary>
        private Dictionary<string, Tuple<string, byte[]>> Files = new Dictionary<string, Tuple<string, byte[]>>();
        private Dictionary<string, byte[]> Values = new Dictionary<string, byte[]>();
        private string _boundary;

        public string ContentType => $"multipart/form-data; boundary={_boundary}";

        public HttpFormData()
        {
            _boundary = Guid.NewGuid().ToString().Replace("-", "");
        }

        private byte[] GetPartHeader(string name, string fileName)
        {
            return Encoding.UTF8.GetBytes($@"--{_boundary}
Content-Disposition: form-data; name=""{name}""; filename=""{fileName}""
Content-Type: application/octet-stream" + "\r\n\r\n");
        }
        private byte[] GetPartHeader(string name)
        {
            return Encoding.UTF8.GetBytes($@"--{_boundary}
Content-Disposition: form-data; name=""{name}""" + "\r\n\r\n");
        }

        public void AddFile(string name, string fileName, string path)
        {
            Files[name] = Tuple.Create(fileName, File.ReadAllBytes(path)
                .Concat(new byte[] { 13, 10 }).ToArray());
        }

        public void AddFileForText(string name, string fileName, string value)
        {
            Files[name] = Tuple.Create(fileName, Encoding.UTF8.GetBytes(value)
                .Concat(new byte[] { 13, 10 }).ToArray());
        }

        public void AddData(string name, string value)
        {
            Values[name] = Encoding.UTF8.GetBytes(value)
                .Concat(new byte[] { 13, 10 }).ToArray();
        }

        public static implicit operator byte[] (HttpFormData @this)
        {
            var content = new byte[0];
            foreach (var data in @this.Values)
            {
                content = content.Concat(@this.GetPartHeader(data.Key))
                    .Concat(data.Value).ToArray();
            }
            foreach (var data in @this.Files)
            {
                content = content.Concat(@this.GetPartHeader(data.Key, data.Value.Item1))
                    .Concat(data.Value.Item2).ToArray();
            }
            content = content.Concat(Encoding.UTF8.GetBytes($"--{@this._boundary}--")).ToArray();

            return content;
        }
    }
}
