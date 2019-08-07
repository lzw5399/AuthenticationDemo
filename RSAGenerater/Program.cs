using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;

namespace RSAGenerater
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateAndSaveKey();

            Console.WriteLine("生成成功");

            Console.ReadKey();
        }

        private static void GenerateAndSaveKey()
        {
            RSAParameters publicKeys, privateKeys;

            // 支持512 1024 2048 4096, 系数越高, 越安全, 但效率也越低
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    privateKeys = rsa.ExportParameters(true);
                    publicKeys = rsa.ExportParameters(false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            var projectPath = new DirectoryInfo("../../../").FullName;

            System.IO.File.WriteAllText(
                Path.Combine(projectPath, "key-private.json"),
                JsonConvert.SerializeObject(privateKeys));

            System.IO.File.WriteAllText(
                Path.Combine(projectPath, "key-public.json"),
                JsonConvert.SerializeObject(publicKeys));
        }
    }
}
