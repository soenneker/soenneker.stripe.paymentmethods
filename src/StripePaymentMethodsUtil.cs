using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.Enums.PaymentMethodTypes;
using Soenneker.Stripe.PaymentMethods.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Stripe;

namespace Soenneker.Stripe.PaymentMethods;

///<inheritdoc cref="IStripePaymentMethodsUtil"/>
public class StripePaymentMethodsUtil : IStripePaymentMethodsUtil
{
    private readonly AsyncSingleton<PaymentMethodService> _service;

    public StripePaymentMethodsUtil(IStripeClientUtil stripeUtil)
    {
        _service = new AsyncSingleton<PaymentMethodService>(async (cancellationToken, _) =>
        {
            StripeClient client = await stripeUtil.Get(cancellationToken).NoSync();

            return new PaymentMethodService(client);
        });
    }

    public async ValueTask<List<PaymentMethod>?> GetAll(CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<PaymentMethod>? response = (await _service.Get(cancellationToken).NoSync()).ListAutoPagingAsync(cancellationToken: cancellationToken);

        if (response == null)
            return null;

        return await response.ToListAsync(cancellationToken).NoSync();
    }

    public async ValueTask<List<PaymentMethod>?> GetAllByUserIdAndType(string userId, StripePaymentMethodType type,
        CancellationToken cancellationToken = default)
    {
        var options = new PaymentMethodListOptions
        {
            Type = type.Value,
            Customer = userId
        };

        IAsyncEnumerable<PaymentMethod>? response =
            (await _service.Get(cancellationToken).NoSync()).ListAutoPagingAsync(options: options, cancellationToken: cancellationToken);

        if (response == null)
            return null;

        return await response.ToListAsync(cancellationToken).NoSync();
    }

    public async ValueTask<List<PaymentMethod>> GetAllByUserIdAndTypes(string userId, List<StripePaymentMethodType> types, CancellationToken cancellationToken = default)
    {
        var all = new List<PaymentMethod>();
        PaymentMethodService service = await _service.Get(cancellationToken).NoSync();

        foreach (StripePaymentMethodType type in types)
        {
            var options = new PaymentMethodListOptions
            {
                Type = type.Value,
                Customer = userId
            };

            List<PaymentMethod>? methods = await service.ListAutoPagingAsync(options, cancellationToken: cancellationToken).ToListAsync(cancellationToken).NoSync();

            if (methods != null)
                all.AddRange(methods);
        }

        return all;
    }

    public async ValueTask<PaymentMethod?> GetFirstByUserIdAndType(string userId, StripePaymentMethodType type, CancellationToken cancellationToken = default)
    {
        var options = new PaymentMethodListOptions
        {
            Type = type.Value,
            Customer = userId,
            Limit = 1
        };

        PaymentMethodService service = await _service.Get(cancellationToken).NoSync();
        StripeList<PaymentMethod>? list = await service.ListAsync(options, cancellationToken: cancellationToken).NoSync();

        return list?.Data?.FirstOrDefault();
    }

    public async ValueTask<PaymentMethod?> Get(string paymentMethodId, CancellationToken cancellationToken = default)
    {
        return await (await _service.Get(cancellationToken).NoSync()).GetAsync(paymentMethodId, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask Delete(string paymentMethodId, CancellationToken cancellationToken = default)
    {
        await (await _service.Get(cancellationToken).NoSync()).DetachAsync(paymentMethodId, cancellationToken: cancellationToken).NoSync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _service.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return _service.DisposeAsync();
    }
}