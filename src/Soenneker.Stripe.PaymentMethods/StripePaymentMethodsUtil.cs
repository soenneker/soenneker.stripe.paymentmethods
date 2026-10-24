using PaymentMethodQuery = Soenneker.Stripe.OpenApiClient.V1.Payment_methods.Item.WithPaymentMethodItemRequestBuilder.WithPaymentMethodItemRequestBuilderGetQueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Stripe.Client;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.OpenApiClient.Models;
using Soenneker.Stripe.PaymentMethods.Abstract;

using Soenneker.Stripe.Enums.PaymentMethodTypes;

namespace Soenneker.Stripe.PaymentMethods;

public sealed class StripePaymentMethodsUtil(IStripeClientUtil client) : IStripePaymentMethodsUtil
{
    public async ValueTask<PaymentMethod> Create(PostPaymentMethodsXWwwFormUrlencodedRequest options, StripeRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_methods.PostAsync(options, c => requestOptions?.Apply(c.Headers), cancellationToken) ?? throw new InvalidOperationException("Stripe returned an empty payment method.");
    }
    public async ValueTask<PaymentMethod> Update(string paymentMethodId, PostPaymentMethodsPaymentMethodXWwwFormUrlencodedRequest options, StripeRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_methods[paymentMethodId].PostAsync(options, c => requestOptions?.Apply(c.Headers), cancellationToken) ?? throw new InvalidOperationException("Stripe returned an empty payment method.");
    }
    public async ValueTask Attach(string paymentMethodId, PostPaymentMethodsPaymentMethodAttachXWwwFormUrlencodedRequest options, StripeRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        await api.V1.Payment_methods[paymentMethodId].Attach.PostAsync(options, c => requestOptions?.Apply(c.Headers), cancellationToken);
    }
    public async ValueTask Delete(string paymentMethodId, PostPaymentMethodsPaymentMethodDetachXWwwFormUrlencodedRequest? options = null, StripeRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        await api.V1.Payment_methods[paymentMethodId].Detach.PostAsync(options ?? new(), c => requestOptions?.Apply(c.Headers), cancellationToken);
    }
    public async ValueTask<PaymentMethod?> Get(string paymentMethodId, PaymentMethodQuery? options = null, StripeRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_methods[paymentMethodId].GetAsync(new(), c => { c.QueryParameters = options ?? new(); requestOptions?.Apply(c.Headers); }, cancellationToken);
    }
    private async ValueTask<List<PaymentMethod>> List(string? customer, StripePaymentMethodType? type, CancellationToken cancellationToken)
    {
        var api = await client.Get(cancellationToken);
        var result = new List<PaymentMethod>();
        string? cursor = null;
        do
        {
            var page = await api.V1.Payment_methods.GetAsync(new(), c =>
            {
                c.QueryParameters.Customer = customer;
                c.QueryParameters.Type = type == null ? null : Microsoft.Kiota.Abstractions.Helpers.EnumHelpers.GetEnumValue<GetPaymentMethodsTypeParameter>(type.Value);
                c.QueryParameters.Limit = 100;
                c.QueryParameters.StartingAfter = cursor;
            }, cancellationToken);
            if (page?.Data is not { Count: > 0 }) break;
            result.AddRange(page.Data);
            cursor = page.HasMore == true ? page.Data[^1].Id : null;
        } while (cursor != null);
        return result;
    }

    public async ValueTask<List<PaymentMethod>?> GetAll(CancellationToken cancellationToken = default) => await List(null, null, cancellationToken);
    public async ValueTask<List<PaymentMethod>?> GetAllByUserIdAndType(string userId, StripePaymentMethodType type, CancellationToken cancellationToken = default) =>
        await List(userId, type, cancellationToken);
    public async ValueTask<List<PaymentMethod>> GetAllByUserIdAndTypes(string userId, List<StripePaymentMethodType> types, CancellationToken cancellationToken = default)
    {
        var result = new List<PaymentMethod>();
        foreach (var type in types) result.AddRange(await List(userId, type, cancellationToken));
        return result;
    }
    public async ValueTask<PaymentMethod?> GetFirstByUserIdAndType(string userId, StripePaymentMethodType type, CancellationToken cancellationToken = default) =>
        (await List(userId, type, cancellationToken)).FirstOrDefault();
    public void Dispose() { }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
