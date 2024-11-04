using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Soenneker.Stripe.PaymentMethods.Abstract;

/// <summary>
/// A .NET typesafe implementation of Stripe's Payment Methods API
/// </summary>
public interface IStripePaymentMethodsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Retrieves all payment methods available in Stripe.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing a list of <see cref="PaymentMethod"/> objects, or null if no payment methods are found.</returns>
    [Pure]
    ValueTask<List<PaymentMethod>?> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all payment methods associated with a specified user ID.
    /// </summary>
    /// <param name="userId">The unique identifier for the user whose payment methods are to be retrieved.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing a list of <see cref="PaymentMethod"/> objects associated with the user, or null if no payment methods are found.</returns>
    [Pure]
    ValueTask<List<PaymentMethod>?> GetAllByUserId(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific payment method by its unique payment method ID.
    /// </summary>
    /// <param name="paymentMethodId">The unique identifier for the payment method.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing the <see cref="PaymentMethod"/> if found, or null if no payment method is found with the specified ID.</returns>
    [Pure]
    ValueTask<PaymentMethod?> Get(string paymentMethodId, CancellationToken cancellationToken = default);
}