using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarsRobots.Entities
{
    public class DictionaryConverter : JsonConverter<Dictionary<int, Dictionary<int, PositionInfo>>>
    {
        public override Dictionary<int, Dictionary<int, PositionInfo>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<int, Dictionary<int, PositionInfo>> res = new Dictionary<int, Dictionary<int, PositionInfo>>();
            res = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, PositionInfo>>>(reader.GetString());
            return res;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<int, Dictionary<int, PositionInfo>> value, JsonSerializerOptions options)
        {
            JsonConvert.SerializeObject<Dictionary<int, Dictionary<int, PositionInfo>>>(value);
            writer.WriteStringValue(value.Add);
        }
    }
}
