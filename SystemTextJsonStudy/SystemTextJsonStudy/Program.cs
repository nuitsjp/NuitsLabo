using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemTextJsonStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new JsonSerializerOptions
            {
                Converters = {new UnitIdConverter()}
            };

            var unit = (IUnit)new Unit(new UnitId(100));
            var unitString = JsonSerializer.Serialize(unit, options);
            Console.WriteLine(unitString);
            var deserializedUnit = JsonSerializer.Deserialize<Unit>(unitString, options);
            Console.WriteLine(deserializedUnit);
        }
    }

    public class UnitIdConverter : JsonConverter<IUnitId>
    {
        public override IUnitId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TryGetInt32(out var value))
            {
                return new UnitId(value);
            }
            else
            {
                return null;
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            IUnitId value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, (IUnitId)null, options);
                    break;
                default:
                {
                    writer.WriteNumberValue(((UnitId)value).Value);
                    //JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    break;
                }
            }
        }
    }

    public interface IUnitId
    {
        int Value { get; }
    }

    public interface IUnit
    {
        IUnitId UnitId { get; }
    }

    public class UnitId : IUnitId
    {
        public UnitId(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class Unit : IUnit
    {
        public Unit(IUnitId unitId)
        {
            UnitId = unitId;
        }

        public IUnitId UnitId { get; }

        public override string ToString() => $"{{\"UnitId\":{UnitId.Value}}}";
    }


}
