using MagicOnion.Server;

namespace MagicOnionStudy.Server;

public class SampleFilterAttribute : MagicOnionFilterAttribute
{
    public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
    {
        var entry = context.CallContext.RequestHeaders.Get("x-foo");
        var value = entry.Value;
        try
        {
            /* on before */
            await next(context); // next
            /* on after */
        }
        catch
        {
            /* on exception */
            throw;
        }
        finally
        {
            /* on finally */
        }
    }
}