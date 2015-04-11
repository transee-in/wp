using System;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Text;

namespace Transee.Common {
    class ColorGenerator {
        public byte R, G, B;

        private string str;

        public ColorGenerator(string str) {
            this.str = str;
        }

        public Windows.UI.Color Generate() {
            this.HashAsRGBA();

            return new Windows.UI.Color() {
                R = this.R, G = this.G, B = this.B, A = 190
            };
        }

        private void HashAsRGBA() {
            var hash = ComputeMD5(this.str);

            this.R = hash[0];
            this.G = hash[1];
            this.B = hash[2];
        }


        public static byte[] ComputeMD5(string str) {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            byte[] value;
            CryptographicBuffer.CopyToByteArray(hashed, out value);
            return value;
        }
    }
}
