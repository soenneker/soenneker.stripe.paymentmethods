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
    public static IServiceCollection AddStripePaymentMethodsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton()
                .TryAddSingleton<IStripePaymentMethodsUtil, StripePaymentMethodsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IStripePaymentMethodsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddStripePaymentMethodsUtilAsScoped(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton()
                .TryAddScoped<IStripePaymentMethodsUtil, StripePaymentMethodsUtil>();

        return services;
    }
}