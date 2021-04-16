using Newtonsoft.Json;
using System;

namespace MaskSensitiveData
{
    public class MaskSensitiveData : JsonConverter
    {
        private const string DEFAULT_MASK = "**********";

        public MaskSensitiveData()
        {
            Mask = DEFAULT_MASK;
        }

        public MaskSensitiveData(string mask = DEFAULT_MASK)
        {
            Mask = mask;
        }

        public string Mask { get; }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(Mask);
        }
    }
}