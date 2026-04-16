using System.Text;
using JulMar.Smpp.Elements;

namespace JulMar.Smpp.Utility
{
    /// <summary>
    /// Maps SMPP data_coding values to System.Text.Encoding instances used for
    /// encoding/decoding the body of short_message and message_payload elements.
    /// </summary>
    public static class EncodingHelper
    {
        /// <summary>
        /// Returns the .NET encoding that should be used for the given SMPP
        /// data_coding. SMSC_DEFAULT is mapped to ISO-8859-1 to preserve the
        /// historical behaviour of this library. For Polish, Cyrillic or any
        /// other text that does not fit in ISO-8859-1, explicitly set
        /// data_coding to DataEncoding.UCS2 on the PDU.
        /// </summary>
        public static Encoding For(DataEncoding dc)
        {
            switch (dc)
            {
                case DataEncoding.IA5:
                    return Encoding.ASCII;
                case DataEncoding.LATIN:
                    return Encoding.GetEncoding("iso-8859-1");
                case DataEncoding.CYRLLIC:
                    return Encoding.GetEncoding("iso-8859-5");
                case DataEncoding.LATINHEBREW:
                    return Encoding.GetEncoding("iso-8859-8");
                case DataEncoding.UCS2:
                    return Encoding.BigEndianUnicode;
                case DataEncoding.JIS:
                    return Encoding.GetEncoding("shift_jis");
                case DataEncoding.ISO2022_JP:
                    return Encoding.GetEncoding("iso-2022-jp");
                case DataEncoding.KS_C_5601:
                    return Encoding.GetEncoding("ks_c_5601-1987");
                case DataEncoding.SMSC_DEFAULT:
                case DataEncoding.OCTET2:
                case DataEncoding.OCTET8:
                case DataEncoding.PICTOGRAM_ENCODING:
                case DataEncoding.EXTENDEDKANJIJIS:
                default:
                    return Encoding.GetEncoding("iso-8859-1");
            }
        }
    }
}
