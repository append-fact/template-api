using System.Globalization;
using Newtonsoft.Json;

namespace Application.Common.Converters
{

    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(@"hh\:mm\:ss"));
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var timeString = (string)reader.Value;
            return TimeSpan.ParseExact(timeString, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }
    }

}
