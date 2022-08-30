using Newtonsoft.Json;
using System;

namespace ConfigReFormatter
{
    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((bool)value ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var val = reader.Value.ToString().ToLower();
            return "1".Equals(val) || "true".Equals(val);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }

    public static class Extension
    {
        public static string ToKv(this bool val) => val ? "true" : "false";
        public static string ToKv(this bool? val, bool def = true) => val.GetValueOrDefault(def) ? "true" : "false";
        public static string ToKvNum(this bool val) => val ? "1" : "0";
        public static string ToKvNum(this bool? val, bool def = true) => val.GetValueOrDefault(def) ? "1" : "0";
    }
}