using System;
using System.IO;
using System.Security.Cryptography;
using Xunit;
using MoonSharp.Extensions;

namespace SecuritySupportTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var rsa = new RSACryptoServiceProvider();
            using (var file = new FileStream("_rsa_xml_public", FileMode.Create))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(rsa.ToXmlStringCore(false));
                }
            }

            using (var file = new FileStream("_rsa_xml_private", FileMode.Create))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(rsa.ToXmlStringCore(true));
                }
            }

            using (var file = new FileStream("_rsa_pem_public", FileMode.Create))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(rsa.ToPemStringCore(false));
                }
            }

            using (var file = new FileStream("_rsa_pem_private", FileMode.Create))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(rsa.ToPemStringCore(true));
                }
            }

        }
    }
}
