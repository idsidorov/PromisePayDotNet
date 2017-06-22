using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromisePayDotNet.Enums;

namespace PromisePayDotNet.Internals
{
    public class FeeToJsonConverter : JsonConverter
    {
        private static readonly IDictionary<string, FeeToType> stringToEnum;
        private static readonly Dictionary<FeeToType, string> enumToString;

        static FeeToJsonConverter()
        {
            stringToEnum = new Dictionary<string, FeeToType>() {
                {"buyer", FeeToType.Buyer},
                {"seller", FeeToType.Seller},
                {"cc", FeeToType.CC},
                {"int_wire", FeeToType.IntWire},
                {"paypal_payout", FeeToType.PaypalPayout},
                {"", FeeToType.None},
            };
            enumToString = stringToEnum.ToDictionary(kv => kv.Value, kv => kv.Key);
        }


        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FeeToType);
        }

        public static FeeToType Parse(string str)
        {
            if (stringToEnum.TryGetValue(str, out var receiver)) return receiver;
            throw new Exception($"Unknown value {str}");
        }

        public static string ToString(FeeToType receiver)
        {
            if (enumToString.TryGetValue(receiver, out var str))
            {
                return str;
            }
            else
            {
                throw new Exception($"Unknown value {receiver}");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Parse((string)serializer.Deserialize(reader, typeof(string)) ?? string.Empty);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ToString((FeeToType)value));
        }
    }
}
