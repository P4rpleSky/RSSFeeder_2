using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MarrubiumShop.Models
{
    public static class JsonDefaultOptions
    {
        public static JsonSerializerOptions Serializer = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
    }
}
