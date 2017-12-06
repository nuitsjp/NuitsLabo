using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace RealProxyConsole
{
    public class AspectWeavingProxy : RealProxy
    {
        private readonly MarshalByRefObject _target;
        public AspectWeavingProxy(MarshalByRefObject target) : base(target.GetType())
        {
            _target = target;
        }

        public AspectWeavingProxy(MarshalByRefObject target, Type serverType) : base(serverType)
        {
            _target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IConstructionCallMessage constructionCallMessage)
            {
                var defaultProxy = RemotingServices.GetRealProxy(this._target);
                defaultProxy.InitializeServerObject(constructionCallMessage);
                var tp = this.GetTransparentProxy() as MarshalByRefObject;
                return EnterpriseServicesHelper.CreateConstructionReturnMessage(constructionCallMessage, tp);
            }
            else if (msg is IMethodCallMessage methodCallMessage)
            {
                var args = methodCallMessage.Args;
                var result = methodCallMessage.MethodBase.Invoke(_target, args);
                return new ReturnMessage(result, args, args.Length, methodCallMessage.LogicalCallContext, methodCallMessage);
            }
            throw new NotImplementedException();
        }
    }
}
