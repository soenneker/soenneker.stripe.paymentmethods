using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Stripe.Client.Registrars;
using Soenneker.Stripe.PaymentMethods.Abstract;

namespace Soenneker.Stripe.PaymentMethods.Registrars;

/// <summary>
/// A .NET typesafe implementation of Stripe's Payment Methods API
/// </summary>
public static class StripePaymentMethodsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IStripePaymentMethodsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static void AddStripePaymentMethodsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton();
        services.TryAddSingleton<IStripePaymentMethodsUtil, StripePaymentMethodsUtil>();
    }

    /// <summary>
    /// Adds <see cref="IStripePaymentMethodsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static void AddStripePaymentMethodsUtilAsScoped(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton();
        services.TryAddScoped<IStripePaymentMethodsUtil, StripePaymentMethodsUtil>();
    }
}
