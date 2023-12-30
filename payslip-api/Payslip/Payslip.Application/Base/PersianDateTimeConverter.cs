using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Payslip.Application.Base
{
    public class PersianDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            DateTime.TryParse(reader.GetString(), CultureInfo.GetCultureInfo("fa-ir"), new DateTimeStyles(), out var result)
                ? result
                : throw new Exception("در متن تاریخ اشکالی وجود دارد!");
        public override void Write(Utf8JsonWriter writer, DateTime dateTimeValue, JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.ToString("yyyy/MM/dd", CultureInfo.GetCultureInfo("fa-ir")));
    }
}
