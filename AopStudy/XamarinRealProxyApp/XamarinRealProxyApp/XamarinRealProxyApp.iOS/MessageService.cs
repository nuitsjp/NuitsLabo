using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using XamarinRealProxyApp.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(MessageService))]
namespace XamarinRealProxyApp.iOS
{
    [WeaveAspect]
    public class MessageService : ContextBoundObject, IMessageService
    {
        public string GetMessage()
        {
            var proxy = new MessangerProxy(new Messanger());
            var messanger = (Messanger) proxy.GetTransparentProxy();
            var message = messanger.GetMessage();
            return message;
        }
    }

    public class MessangerProxy : RealProxy
    {
        private readonly Messanger _messanger;
        public MessangerProxy(Messanger messanger) : base(typeof(Messanger))
        {
            _messanger = messanger;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCallMessage = (IMethodCallMessage) msg;
            var args = methodCallMessage.Args;
            var result = methodCallMessage.MethodBase.Invoke(_messanger, args);
            return new ReturnMessage(result, args, args.Length, methodCallMessage.LogicalCallContext, methodCallMessage);
        }
    }

    public class Messanger : MarshalByRefObject
    {
        public string GetMessage()
        {
            return "Hello, iOS";
        }
    }

}
