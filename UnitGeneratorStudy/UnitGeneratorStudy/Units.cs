using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using UnitGenerator;

namespace UnitGeneratorStudy;

[UnitOf(typeof(Guid))]
public readonly partial struct GroupId { }

[UnitOf(typeof(string))]
public readonly partial struct Message { }

[UnitOf(typeof(long))]
public readonly partial struct Power { }

[UnitOf(typeof(byte[]))]
public readonly partial struct Image { }

[UnitOf(typeof(DateTime), UnitGenerateOptions.None, "{0:yyyy年MM月dd日}")]
public readonly partial struct StartDate { }

[UnitOf(typeof((string street, string city)), UnitGenerateOptions.None, "{0}")]
public readonly partial struct StreetAddress { }

[TypeConverter(typeof(MyDateTypeConverter))]
public record MyDateTime(DateTime Value)
{
    private class MyDateTypeConverter : TypeConverter
    {
        private static readonly Type WrapperType = typeof(StartDate);

        private static readonly Type ValueType = typeof(DateTime);

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == WrapperType || sourceType == ValueType)
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == WrapperType || destinationType == ValueType)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
        {
            if (value != null)
            {
                Type t = value.GetType();
                if (t == typeof(StartDate))
                {
                    return (StartDate)value;
                }
                if (t == typeof(DateTime))
                {
                    return new StartDate((DateTime)value);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is StartDate wrappedValue)
            {
                if (destinationType == WrapperType)
                {
                    return wrappedValue;
                }
                if (destinationType == ValueType)
                {
                    return wrappedValue.AsPrimitive();
                }
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }


}