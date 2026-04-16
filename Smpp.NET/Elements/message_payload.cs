using System;
using JulMar.Smpp.Utility;

namespace JulMar.Smpp.Elements
{
	/// <summary>
	/// The message_payload parameter contains the user data.
	/// </summary>
	public class message_payload : TlvParameter
	{
		/// <summary>
		/// SMPP element tag
		/// </summary>
		public const short TlvTag = ParameterTags.TAG_MESSAGE_PAYLOAD;

		private DataEncoding dataCoding_ = DataEncoding.SMSC_DEFAULT;

		/// <summary>
		/// Constructor
		/// </summary>
		public message_payload() : base(TlvTag)
		{
		}

		/// <summary>
		/// Parameterized constructor
		/// </summary>
		public message_payload(params byte[] data) : base(TlvTag)
		{
			BinaryValue = data;
		}

		/// <summary>
		/// Parameterized constructor
		/// </summary>
		public message_payload(string data) : base(TlvTag)
		{
			TextValue = data;
		}

		/// <summary>
		/// The SMPP data_coding that determines how TextValue is encoded.
		/// The enclosing PDU sets this from its own data_coding field.
		/// </summary>
		public DataEncoding DataCoding
		{
			get { return dataCoding_; }
			set { dataCoding_ = value; }
		}

		/// <summary>
		/// Returns the length of the data in bytes.
		/// </summary>
		public new int Length
		{
			get { return (int) Data.Length; }
		}

		/// <summary>
		/// This attempts to return the data in string form, decoded with DataCoding.
		/// </summary>
		public string TextValue
		{
			get
			{
				if (Data.Length == 0)
					return string.Empty;
				SmppReader reader = new SmppReader(Data, true);
				byte[] bytes = reader.ReadBytes(Data.Length);
				return EncodingHelper.For(dataCoding_).GetString(bytes);
			}

			set
			{
				// Reset the backing buffer so that writing a shorter value does
				// not leave trailing bytes from a previous, longer value.
				Data = new SmppByteStream();
				if (!string.IsNullOrEmpty(value))
				{
					byte[] bytes = EncodingHelper.For(dataCoding_).GetBytes(value);
					SmppWriter writer = new SmppWriter(Data);
					writer.Add(bytes);
				}
			}
		}

		/// <summary>
		/// Assigns the text payload using the given data_coding in one call.
		/// Equivalent to: DataCoding = dc; TextValue = text;
		/// </summary>
		public void SetText(string text, DataEncoding dc)
		{
			dataCoding_ = dc;
			TextValue = text;
		}

		/// <summary>
		/// This returns the buffer as a binary block.
		/// </summary>
		public byte[] BinaryValue
		{
			get
			{
				SmppReader reader = new SmppReader(Data, true);
				return reader.ReadBytes(Data.Length);
			}

			set
			{
				Data = new SmppByteStream();
				if (value != null && value.Length > 0)
				{
					SmppWriter writer = new SmppWriter(Data);
					writer.Add(value);
				}
			}
		}
	}
}
