using System;

namespace ValueObjectStudy
{
    [ValueObject]
    [ValueObjectMember(typeof(string), "firstName")]
    [ValueObjectMember(typeof(string), "lastName")]
    public partial class Name
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ValueObjectAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ValueObjectMemberAttribute : Attribute
    {
        private readonly Type _type;
        private readonly string _name;

        public ValueObjectMemberAttribute(Type type, string name)
        {
            _type = type;
            _name = name;
        }
    }

}
