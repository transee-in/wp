using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI;

namespace Transee.Common {
    class ColorGenerator {
        public byte R, G, B;

        private readonly string _str;

        public ColorGenerator(string str) {
            _str = str;
        }

        public Color Generate() {
            HashAsRgba();

            return new Color() {
                R = R, G = G, B = B, A = 190
            };
        }

        private void HashAsRgba() {
            var hash = ComputeMd5(_str);

            R = hash[0];
            G = hash[1];
            B = hash[2];
        }


        public static byte[] ComputeMd5(string str) {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            byte[] value;
            CryptographicBuffer.CopyToByteArray(hashed, out value);
            return value;
        }
    }
}
