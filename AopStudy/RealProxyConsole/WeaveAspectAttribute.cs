using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace RealProxyConsole
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
