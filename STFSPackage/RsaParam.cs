using System;
using System.Security.Cryptography;
using Dark_Souls_II_Save_Editor.Properties;
using HavenInterface.IOPackage;
using HavenInterface.Utils;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    /// <summary>Contains function/s that deals with RSA Parameter to fix the CON file</summary>
    public class RsaParam
    {
        private RSAParameters _parameters;

        /// <summary>Initialize the RSAParameters to fix the xbox 360 Con</summary>
        internal RsaParam()
        {
            try
            {
                var kv = new StreamIO(Resources.KV, false, true);
                LoadCon(kv);
                kv.Close();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>console package signature generation</summary>
        /// <param name="kv">The key vault input stream</param>
        private void LoadCon(StreamIO kv)
        {
            var offset = 0;
            if (kv.Length == 0x4000L)
            {
                offset = 0x10;
            }
            else if (kv.Length != 0x3ff0L)
            {
                throw new Exception("Key vault not the correct size");
            }
            kv.Position = 0x9b8 + offset;
            Certificate = kv.ReadBytes(0x1a8);
            _parameters.D =
                XFunctions.HexToBytes(
                    "51EC1F9D5626C2FC10A66764CB3A6D4DA1E74EA842F0F4FDFA66EFC78E102FE41CA31DD0CE392EC3192DD0587479AC08E790C1AC2DC6EB47E83DCF4C6DFF5165D46EBD0F15793795C4AF909E2B508A0A224AB341E5898073CDFA2102F5DD30DD072A6F340781977EB2FB72E9EAC18839AC482BA84DFCD7ED9BF9DEC245934C4C");
            kv.Position = 0x28c + offset;
            _parameters.Exponent = kv.ReadBytes(4);
            kv.Position = 0x298 + offset;
            _parameters.Modulus = kv.ReadBytes(0x80).SimpleScramble(false);
            _parameters.P = kv.ReadBytes(0x40).SimpleScramble(false);
            _parameters.Q = kv.ReadBytes(0x40).SimpleScramble(false);
            _parameters.DP = kv.ReadBytes(0x40).SimpleScramble(false);
            _parameters.DQ = kv.ReadBytes(0x40).SimpleScramble(false);
            _parameters.InverseQ = kv.ReadBytes(0x40).SimpleScramble(false);
            if (BitConverter.ToString(Certificate.GetBytePiece(0x28, 0x80).SimpleScramble(false)) !=
                BitConverter.ToString(_parameters.Modulus))
                throw new Exception("Invalid Certificate Key!");
            try
            {
                var x = new RSACryptoServiceProvider();
                x.ImportParameters(_parameters);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        #region properties

        /// <summary>Get Certificate</summary>
        internal byte[] Certificate { get; private set; }

        /// <summary>Get the RSA Parameters for this instance (only works if valid)</summary>
        internal RSAParameters ParametersRsaKeys => _parameters;

        #endregion
    }
}