using System.Collections.Generic;

namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public static class Constants
    {
        public static class TextPosition
        {
            public const string Left = "LEFT";
            public const string Center = "CENTER";
            public const string Right = "RIGHT";

            public static IReadOnlyList<string> All = new string[] {Left, Center, Right};
        }
        
        public static class BlockType
        {
            public const string Logo = "LOGO";
            public const string Date = "DATE";
            public const string Code = "CODE";
            public const string Text = "TEXT";

            public static IReadOnlyList<string> All = new string[] {Logo, Date, Code, Text};
        }
    }
}