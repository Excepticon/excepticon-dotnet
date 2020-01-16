using Excepticon.Options;
using Excepticon.Services;

namespace Excepticon.Integrations
{
    public interface ISdkIntegration
    {
        void Register(IExcepticonClient client, ExcepticonOptions options);

        void Unregister(IExcepticonClient client);
    }
}
