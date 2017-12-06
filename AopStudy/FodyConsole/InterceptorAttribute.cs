using FodyConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

[module: Interceptor]
namespace FodyConsole
{
	// Any attribute which provides OnEntry/OnExit/OnException with proper args
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Assembly | AttributeTargets.Module)]
	public class InterceptorAttribute : Attribute, IMethodDecorator
	{
		// instance, method and args can be captured here and stored in attribute instance fields
		// for future usage in OnEntry/OnExit/OnException
		public void Init(object instance, MethodBase method, object[] args)
		{
			Console.WriteLine(string.Format("Init: {0} [{1}]", method.DeclaringType.FullName + "." + method.Name, args.Length));
		}
		public void OnEntry()
		{
			Console.WriteLine("OnEntry");
		}

		public void OnExit()
		{
			Console.WriteLine("OnExit");
		}

		public void OnException(Exception exception)
		{
			Console.WriteLine(string.Format("OnException: {0}: {1}", exception.GetType(), exception.Message));
		}
	}
}
