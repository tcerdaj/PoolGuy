using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors.Internals;

namespace PoolGuy.Mobile.Behaviors
{
    public class RequiredInputValidationBehavior : ValidationBehavior
    {
        
        protected override ValueTask<bool> ValidateAsync(object value, CancellationToken token)
        {
            return new ValueTask<bool>(!string.IsNullOrEmpty(value?.ToString()));
        }
    }
}