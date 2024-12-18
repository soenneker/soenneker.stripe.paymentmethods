using Soenneker.Stripe.PaymentMethods.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Stripe.PaymentMethods.Tests;

[Collection("Collection")]
public class StripePaymentMethodsUtilTests : FixturedUnitTest
{
    private readonly IStripePaymentMethodsUtil _util;

    public StripePaymentMethodsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IStripePaymentMethodsUtil>();
    }
}
