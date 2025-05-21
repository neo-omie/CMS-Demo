using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CMS.Domain.Converters
{
    public class DateOnlyJsonConverter:JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && DateOnly.TryParseExact(reader.GetString(), DateFormat, out var dateOnly)) 
            {
                return dateOnly;
            }
            throw new JsonException("Invalid DateOnly format.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}
