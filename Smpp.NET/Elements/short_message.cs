using System;
using System.Text;
using JulMar.Smpp.Utility;

namespace JulMar.Smpp.Elements
{
	/// <summary>
	/// The short_message parameter contains the user data.  A maximum of 254 octets can be
	/// sent.  ESME's should use the optional message_payload parameter in submit_sm, submit_multi,
	/// or deliver_sm to send larger data sizes.
	///
	/// NOTE: We deviate from the element definition slightly in that we include the "sm_length"
	/// element as part of this element.  They always go together in this instance.
	/// </summary>
	public class short_message : SmppOctetString
	{
		/// <summary>
		/// The max length of a short-message data component (in bytes on the wire).
		/// </summary>
		public const int MAX_LENGTH = 254;

		private DataEncoding dataCoding_ = DataEncoding.SMSC_DEFAULT;

		/// <summary>
		/// Default constructor
		/// </summary>
		public short_message() : base()
		{
		}

		/// <summary>
		/// Parameterized constructor
		/// </summary>
		/// <param name="s">Value</param>
		public short_message(string s) : base(s)
		{
		}

		/// <summary>
		/// The SMPP data_coding that determines how Value is encoded on the wire.
		/// The enclosing PDU (submit_sm, deliver_sm, etc.) sets this from its own
		/// data_coding field before the element is serialised/deserialised.
		/// </summary>
		public DataEncoding DataCoding
		{
			get { return dataCoding_; }
			set { dataCoding_ = value; }
		}

		/// <summary>
		/// Returns the length of the message in BYTES after encoding with the
		/// currently selected DataCoding, i.e. the value that goes into sm_length
		/// on the wire.
		/// </summary>
		public int Length
		{
			get
			{
				if (string.IsNullOrEmpty(Value))
					return 0;
				return EncodingHelper.For(dataCoding_).GetByteCount(Value);
			}
		}

		/// <summary>
		/// Returns the base message value
		/// </summary>
		public string TextValue
		{
			get { return base.Value; }
			set { base.Value = value; }
		}

		/// <summary>
		/// Returns the message as an array of bytes encoded according to DataCoding.
		/// </summary>
		public byte[] BinaryValue
		{
			get
			{
				if (string.IsNullOrEmpty(Value))
					return new byte[0];
				return EncodingHelper.For(dataCoding_).GetBytes(Value);
			}

			set
			{
				Value = (value == null || value.Length == 0)
					? string.Empty
					: EncodingHelper.For(dataCoding_).GetString(value);
			}
		}

		/// <summary>
		/// This method validates the data in the element.
		/// </summary>
		protected override void ValidateData()
		{
			if (string.IsNullOrEmpty(Value))
				return;

			int byteLen = EncodingHelper.For(dataCoding_).GetByteCount(Value);
			if (byteLen > MAX_LENGTH)
				throw new ArgumentException(
					"short_message too long - it must be <= " + MAX_LENGTH
					+ " bytes (was " + byteLen + ")");
		}

		/// <summary>
		/// Serialises the short_message to the wire as: one-byte length followed by
		/// the encoded bytes (no null terminator). The length is measured in BYTES
		/// using the encoding selected by DataCoding, not in .NET string characters.
		/// </summary>
		public override void AddToStream(SmppWriter writer)
		{
			Encoding enc = EncodingHelper.For(dataCoding_);
			byte[] bytes = string.IsNullOrEmpty(Value) ? new byte[0] : enc.GetBytes(Value);

			if (bytes.Length > MAX_LENGTH)
				throw new ArgumentException(
					"short_message too long - it must be <= " + MAX_LENGTH
					+ " bytes (was " + bytes.Length + ")");

			writer.Add((byte)bytes.Length);
			if (bytes.Length > 0)
				writer.Add(bytes);
		}

		/// <summary>
		/// Reads sm_length then that many bytes and decodes them using DataCoding.
		/// </summary>
		public override void GetFromStream(SmppReader reader)
		{
			int len = reader.ReadByte();
			if (len > 0)
			{
				byte[] bytes = reader.ReadBytes(len);
				TextValue = EncodingHelper.For(dataCoding_).GetString(bytes);
			}
			else
			{
				TextValue = string.Empty;
			}
		}

		/// <summary>
		/// Override of the object.ToString method
		/// </summary>
		public override string ToString()
		{
			int chars = string.IsNullOrEmpty(Value) ? 0 : Value.Length;
			return string.Format("short_message: len={0}, \"{1}\"", chars, Value);
		}
	}
}
