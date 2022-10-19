using MagicOnion.Server;
using MagicOnion;

namespace MagicOnionStudy.Server;

// Implements RPC service in the server project.
// The implementation class must inherit `ServiceBase<IMyFirstService>` and `IMyFirstService`
public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
{
    // `UnaryResult<T>` allows the method to be treated as `async` method.
#pragma warning disable CS1998
    public async UnaryResult<int> SumAsync(int x, int y)
#pragma warning restore CS1998
    {
        Console.WriteLine($"Received:{x}, {y}");
        return x + y;
    }
}