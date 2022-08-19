using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace Mosaico.Tools.CommandLine.Models
{
    public class DateTimeOffsetTypeConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return string.IsNullOrWhiteSpace(text) ? null : DateTimeOffset.ParseExact(text, new string[]{"dd.MM.yyyy"}, null);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TokenomyImportModel
    {
        [Index(0)]
        public string ProjectCode { get; set; }
        
        [Index(1)]
        public string Item { get; set; }
        
        [Index(2)]
        public decimal? Tokens { get; set; }
        
        [Index(3)]
        public decimal? TokenPrice { get; set; }
        
        [Index(4)]
        [TypeConverter(typeof(DateTimeOffsetTypeConverter))]
        public DateTimeOffset? StartDate { get; set; }
        
        [Index(5)]
        [TypeConverter(typeof(DateTimeOffsetTypeConverter))]
        public DateTimeOffset? EndDate { get; set; }
    }
}