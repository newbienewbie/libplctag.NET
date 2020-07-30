﻿using System;
using System.Text;

namespace libplctag.DataTypes
{
    public class StringMarshaller : IMarshaller<string>
    {

        const int MAX_CONTROLLOGIX_STRING_LENGTH = 82;
        const int MAX_LOGIXPCCC_STRING_LENGTH = 80;

        public PlcType PlcType { get; set; }

        public int? ElementSize
        {
            get
            {
                switch (PlcType)
                {
                    case PlcType.ControlLogix: return 88;
                    case PlcType.Plc5: return 88;
                    case PlcType.Slc500: return 25;
                    case PlcType.LogixPccc: return 84;
                    case PlcType.Micro800: return 256;
                    case PlcType.MicroLogix: return 256;
                    default: throw new NotImplementedException();
                }
            }
        }


        public string Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            switch (PlcType)
            {
                case PlcType.ControlLogix: return ControlLogixDecode(tag, offset);
                case PlcType.Plc5: throw new NotImplementedException();
                case PlcType.Slc500: throw new NotImplementedException();
                case PlcType.LogixPccc: return LogixPcccDecode(tag, offset);
                case PlcType.Micro800: throw new NotImplementedException();
                case PlcType.MicroLogix: throw new NotImplementedException();
                default: throw new NotImplementedException();
            }
        }

        public void Encode(Tag tag, int offset, out int elementSize, string value)
        {
            elementSize = ElementSize.Value;
            switch (PlcType)
            {
                case PlcType.ControlLogix: ControlLogixEncode(tag, offset, value); break;
                case PlcType.Plc5: throw new NotImplementedException();
                case PlcType.Slc500: throw new NotImplementedException();
                case PlcType.LogixPccc: LogixPcccEncode(tag, offset, value); break;
                case PlcType.Micro800: throw new NotImplementedException();
                case PlcType.MicroLogix: throw new NotImplementedException();
                default: break;
            }
        }



        


        string ControlLogixDecode(Tag tag, int offset)
        {
            var apparentStringLength = tag.GetInt32(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_CONTROLLOGIX_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];
            for (int ii = 0; ii < actualStringLength; ii++)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 4 + 2 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }

        void ControlLogixEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_CONTROLLOGIX_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int i = 0; i < asciiEncodedString.Length; i++)
            {
                tag.SetUInt8(offset + i + 2 + 2, Convert.ToByte(asciiEncodedString[i]));
            }
        }



        string LogixPcccDecode(Tag tag, int offset)
        {
            var apparentStringLength = (int)tag.GetInt16(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_LOGIXPCCC_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];

            for (int ii = 0; ii < asciiEncodedString.Length; ii += 2)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 4 + ii + 1);
                asciiEncodedString[ii + 1] = tag.GetUInt8(offset + 4 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }

        void LogixPcccEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_LOGIXPCCC_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int ii = 0; ii < asciiEncodedString.Length; ii += 2)
            {
                tag.SetUInt8(offset + ii + 4, Convert.ToByte(asciiEncodedString[ii]));
                tag.SetUInt8(offset + ii + 4 + 1, Convert.ToByte(asciiEncodedString[ii+1]));
            }
        }

    }
}
