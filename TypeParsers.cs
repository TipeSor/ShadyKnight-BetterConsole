namespace BetterConsole
{
    [TypeParser(typeof(short))]
    public class ShortParser : TypeParser<short>
    {
        public override bool TryParse(string input, out short value)
        {
            return short.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(ushort))]
    public class UShortParser : TypeParser<ushort>
    {
        public override bool TryParse(string input, out ushort value)
        {
            return ushort.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(int))]
    public class IntParser : TypeParser<int>
    {
        public override bool TryParse(string input, out int value)
        {
            return int.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(uint))]
    public class UIntParser : TypeParser<uint>
    {
        public override bool TryParse(string input, out uint value)
        {
            return uint.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(long))]
    public class LongParser : TypeParser<long>
    {
        public override bool TryParse(string input, out long value)
        {
            return long.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(ulong))]
    public class ULongParser : TypeParser<ulong>
    {
        public override bool TryParse(string input, out ulong value)
        {
            return ulong.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(float))]
    public class FloatParser : TypeParser<float>
    {
        public override bool TryParse(string input, out float value)
        {
            return float.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(double))]
    public class DoubleParser : TypeParser<double>
    {
        public override bool TryParse(string input, out double value)
        {
            return double.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(byte))]
    public class ByteParser : TypeParser<byte>
    {
        public override bool TryParse(string input, out byte value)
        {
            return byte.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(bool))]
    public class BoolParser : TypeParser<bool>
    {
        public override bool TryParse(string input, out bool value)
        {
            return bool.TryParse(input, out value);
        }
    }

    [TypeParser(typeof(string))]
    public class StringParser : TypeParser<string>
    {
        public override bool TryParse(string input, out string value)
        {
            value = input;
            return true;
        }
    }
}
