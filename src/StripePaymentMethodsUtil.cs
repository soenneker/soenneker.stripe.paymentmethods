using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Stripe.Client.Abstract;
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

            var service = new PaymentMethodService(client);
            return service;
        });
    }

    public async ValueTask<List<PaymentMethod>?> GetAll(CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<PaymentMethod>? response = (await _service.Get(cancellationToken).NoSync()).ListAutoPagingAsync(cancellationToken: cancellationToken);

        if (response == null)
            return null;

        List<PaymentMethod> result = await response.ToListAsync(cancellationToken).NoSync();

        return result;
    }

    public async ValueTask<List<PaymentMethod>?> GetAllByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var options = new PaymentMethodListOptions
        {
            Type = "card",
            Customer = userId
        };

        IAsyncEnumerable<PaymentMethod>? response = (await _service.Get(cancellationToken).NoSync()).ListAutoPagingAsync(options: options, cancellationToken: cancellationToken);

        if (response == null)
            return null;

        List<PaymentMethod> result = await response.ToListAsync(cancellationToken).NoSync();

        return result;
    }

    public async ValueTask<PaymentMethod?> Get(string paymentMethodId, CancellationToken cancellationToken = default)
    {
        PaymentMethod response = await (await _service.Get(cancellationToken).NoSync()).GetAsync(paymentMethodId, cancellationToken: cancellationToken).NoSync();

        return response;
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