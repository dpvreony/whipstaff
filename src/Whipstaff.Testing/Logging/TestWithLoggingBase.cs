using Microsoft.Extensions.Logging;
using Xunit;

namespace Whipstaff.Testing.Logging;

public abstract class TestWithLoggingBase
{
    protected TestWithLoggingBase(ITestOutputHelper output)
    {
        Log = new TestLogger(output);
        Logger = Log.CreateLogger(GetType());
    }

    protected ILogger Logger { get; }

    protected TestLogger Log { get; }
}
