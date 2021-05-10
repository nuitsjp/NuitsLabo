using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace DddAdventureWorks
{
    [TypeConverter(typeof(SalesOrderIDTypeConverter))]
    public interface ISalesOrderID
    {
    }

    public class SalesOrderIDTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new SalesOrderID(int.Parse(value.ToString()));
        }
    }

    public class SalesOrderID : ISalesOrderID
    {
        public SalesOrderID(int value)
        {
            Value = value;
        }

        internal int Value { get; }

    }
}