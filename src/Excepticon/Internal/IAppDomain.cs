using System;

namespace Excepticon.Internal
{
    internal interface IAppDomain
    {
        event UnhandledExceptionEventHandler UnhandledException;

        event EventHandler ProcessExit;
    }
}
