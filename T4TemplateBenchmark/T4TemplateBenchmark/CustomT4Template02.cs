﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン: 16.0.0.0
//  
//     このファイルへの変更は、正しくない動作の原因になる可能性があり、
//     コードが再生成されると失われます。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace T4TemplateBenchmark
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class CustomT4Template02 : MyBaseTemplate02
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("using System;\r\n\r\nnamespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write("\r\n{\r\n    ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Accessibility));
            this.Write(" partial ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Type));
            this.Write(" ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" : IComparable, IComparable<");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(">\r\n    {\r\n#nullable disable\r\n        public int CompareTo(object other)\r\n#nullabl" +
                    "e enable\r\n        {\r\n            if (other is null) return 1;\r\n\r\n            if " +
                    "(other is ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" concreteObject)\r\n            {\r\n                return CompareTo(concreteObject)" +
                    ";\r\n            }\r\n\r\n            throw new ArgumentException(\"Object is not a ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write(".");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(".\");\r\n        }\r\n\r\n");

if(Type == "class") {

            this.Write("#nullable disable\r\n");

}

            this.Write("        public int CompareTo(");
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            this.Write(" other)\r\n");

if(Type == "class") {

            this.Write("#nullable enable\r\n");

}

            this.Write("        {\r\n");

if(Type == "class") {

            this.Write(@"            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

");

}
if(1 < Members.Count)
{

            this.Write("            int compared;\r\n\r\n");

}
var last = Members.Last();
foreach(var member in Members) { 

 
    if(ReferenceEquals(member, last)) { 
        if(Type == "class") {

            this.Write("            return LocalCompareTo(");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(", other.");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(");\r\n");
 
        } 
        else 
        {

            this.Write("            return ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(".CompareTo(other.");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(");\r\n");

        }
    } 
    else 
    { 
        if(Type == "class") {

            this.Write("            compared = LocalCompareTo(");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(", other.");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(");\r\n");

        }
        else 
        {

            this.Write("            compared = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(".CompareTo(other.");
            this.Write(this.ToStringHelper.ToStringWithCulture(member));
            this.Write(");\r\n");

        }

            this.Write("            if (compared != 0) return compared;\r\n\r\n");
 
    }
}

            this.Write("        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
}
