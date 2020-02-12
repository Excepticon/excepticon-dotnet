using System.ComponentModel;
using Excepticon.Options;
using Excepticon.Services;

namespace Excepticon.Integrations
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISdkIntegration
    {
        void Register(IExcepticonClient client, ExcepticonOptions options);

        void Unregister(IExcepticonClient client);
    }
}
