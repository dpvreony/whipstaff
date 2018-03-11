namespace Dhgms.AspNetCoreContrib.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Taken from
    /// https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
    /// https://raw.githubusercontent.com/negativeeddy/blog-examples/master/ConfigureAwaitBehavior/ExtremeConfigAwaitLibrary/SynchronizationContextRemover.cs
    /// </remarks>
    internal struct SynchronizationContextRemover : INotifyCompletion
    {
        public bool IsCompleted => SynchronizationContext.Current == null;

        public void OnCompleted(Action continuation)
        {
            var prevContext = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                continuation();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevContext);
            }
        }

        public SynchronizationContextRemover GetAwaiter()
        {
            return this;
        }

        public void GetResult()
        {
            // there is no op here. it's just here to ensure execution with no context
            // it's used by await
        }
    }
}
