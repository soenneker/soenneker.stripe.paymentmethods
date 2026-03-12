using Soenneker.Stripe.Enums.PaymentMethodTypes;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Stripe.PaymentMethods.Abstract;

/// <summary>
/// A .NET typesafe implementation of Stripe's Payment Methods API, providing access to create, retrieve, and delete customer payment methods.
/// </summary>
public interface IStripePaymentMethodsUtil : IAsyncDisposable, IDisposable
{
    ValueTask<PaymentMethod> Create(PaymentMethodCreateOptions options, RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all available payment methods in the connected Stripe account. This is not scoped to a specific customer.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing a list of <see cref="PaymentMethod"/> objects, or <c>null</c> if none are found.</returns>
    [Pure]
    ValueTask<List<PaymentMethod>?> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all payment methods of a specific type associated with the given Stripe customer ID.
    /// </summary>
    /// <param name="userId">The Stripe customer ID.</param>
    /// <param name="type">The Stripe payment method type to filter by (e.g., <c>card</c>).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing a list of <see cref="PaymentMethod"/> objects for the customer, or <c>null</c> if none are found.</returns>
    [Pure]
    ValueTask<List<PaymentMethod>?> GetAllByUserIdAndType(string userId, StripePaymentMethodType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all payment methods associated with the given Stripe customer ID across multiple specified payment method types.
    /// </summary>
    /// <param name="userId">The Stripe customer ID.</param>
    /// <param name="types">A list of Stripe payment method types to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing a combined list of <see cref="PaymentMethod"/> objects across all requested types.</returns>
    [Pure]
    ValueTask<List<PaymentMethod>> GetAllByUserIdAndTypes(string userId, List<StripePaymentMethodType> types, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the first available payment method of a specific type for a given Stripe customer ID.
    /// </summary>
    /// <param name="userId">The Stripe customer ID.</param>
    /// <param name="type">The type of payment method to retrieve (e.g., <c>card</c>).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing the first <see cref="PaymentMethod"/> found, or <c>null</c> if none exist.</returns>
    [Pure]
    ValueTask<PaymentMethod?> GetFirstByUserIdAndType(string userId, StripePaymentMethodType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific payment method by its Stripe payment method ID.
    /// </summary>
    /// <param name="paymentMethodId">The Stripe ID of the payment method to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> containing the <see cref="PaymentMethod"/> if found, or <c>null</c> otherwise.</returns>
    [Pure]
    ValueTask<PaymentMethod?> Get(string paymentMethodId, PaymentMethodGetOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);

    ValueTask<PaymentMethod> Update(string paymentMethodId, PaymentMethodUpdateOptions options, RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Detaches (removes) a payment method from a Stripe customer. This does not delete the payment method from Stripe entirely.
    /// </summary>
    /// <param name="paymentMethodId">The Stripe ID of the payment method to detach.</param>
    /// <param name="requestOptions"></param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <param name="options"></param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask Delete(string paymentMethodId, PaymentMethodDetachOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);

    ValueTask Attach(string paymentMethodId, PaymentMethodAttachOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
}