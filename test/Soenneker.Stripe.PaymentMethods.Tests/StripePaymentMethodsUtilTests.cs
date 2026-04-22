using Soenneker.Stripe.PaymentMethods.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Stripe.PaymentMethods.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class StripePaymentMethodsUtilTests : HostedUnitTest
{
    private readonly IStripePaymentMethodsUtil _util;

    public StripePaymentMethodsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IStripePaymentMethodsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
