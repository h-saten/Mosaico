using System.Linq;
using System.Text.RegularExpressions;

namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class BaseTextBlock : BaseBlock
    {
        public decimal FontSizePx { get; set; }
        public string FontColor { get; set; }
        public bool TextBold { get; set; }
        public string TextAlign { get; set; }

        public new bool IsValid()
        {
            var colorRegex = new Regex(@"^#(?:[0-9a-fA-F]{3}){1,2}$");
            return base.IsValid() && Constants.TextPosition.All.Contains(TextAlign) && FontSizePx > 0 && FontSizePx < 500 && colorRegex.IsMatch(FontColor);
        }
    }
}