using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Options;

namespace Mosaico.PDF.Base.Generator.Models
{
    public class CustomConvertOptions : ConvertOptions
    {
        [OptionFlag("--keep-relative-links")]
        public bool KeepRelative { get; set; }
    }
}