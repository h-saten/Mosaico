namespace Mosaico.Blockchain.Base.Extensions
{
    public static class NumberExtensions
    {
        public static string ToHex(this int val)
        {
            return $"0x{val:X}";
        }
        
        public static string ToHex(this decimal val)
        {
            return $"0x{val:X}";
        }
        
        public static string ToHex(this long val)
        {
            return $"0x{val:X}";
        }
    }
}