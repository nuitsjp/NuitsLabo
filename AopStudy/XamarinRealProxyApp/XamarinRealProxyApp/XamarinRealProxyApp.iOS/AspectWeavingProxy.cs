using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Text;

namespace XamarinRealProxyApp.iOS
{
    public class AspectWeavingProxy : RealProxy
    {
        private readonly MarshalByRefObject _target;
        public AspectWeavingProxy(MarshalByRefObject target, Type serverType) : base(serverType)
        {
            _target = target;
        }
        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IConstructionCallMessage constructionCallMessage)
            {
                RealProxy rp = RemotingServices.GetRealProxy(this._target);
                var res = rp.InitializeServerObject(constructionCallMessage);
                var tp = this.GetTransparentProxy() as MarshalByRefObject;
                return EnterpriseServicesHelper.CreateConstructionReturnMessage(constructionCallMessage, tp);
            }
            else if(msg is IMethodCallMessage methodCallMessage)
            {
                var args = methodCallMessage.Args;
                var result = methodCallMessage.MethodBase.Invoke(_target, args);
                return new ReturnMessage(result, args, args.Length, methodCallMessage.LogicalCallContext, methodCallMessage);
            }
            throw new NotImplementedException();
        }
    }
}
