using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Cauldron.Interception;

namespace XFodyApp
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class InterceptorAttribute : Attribute, IMethodInterceptor
    {
        public InterceptorAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public void OnEnter(Type declaringType, object instance, MethodBase methodbase, object[] values)
        {
            Debug.WriteLine($"OnEnter declaringType:{declaringType} instance:{instance} values:{string.Join(", ", values)}");
        }

        public void OnException(Exception e)
        {
        }

        public void OnExit()
        {
            Debug.WriteLine("OnExit");
        }
    }
}
