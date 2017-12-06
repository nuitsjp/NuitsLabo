using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace XamarinRealProxyApp.iOS
{
    public class WeaveAspectAttribute : ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            MarshalByRefObject target = base.CreateInstance(serverType);
            return (MarshalByRefObject)new AspectWeavingProxy(target, serverType).GetTransparentProxy();
        }
    }
}
