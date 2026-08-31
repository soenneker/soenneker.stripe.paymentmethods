[![](https://img.shields.io/nuget/v/soenneker.stripe.paymentmethods.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stripe.paymentmethods/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stripe.paymentmethods/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.stripe.paymentmethods/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.stripe.paymentmethods.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stripe.paymentmethods/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stripe.paymentmethods/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.stripe.paymentmethods/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Stripe.PaymentMethods

Create, retrieve, update, attach, detach, and auto-page through Stripe payment methods with customer and type filtering.

## Installation

```bash
dotnet add package Soenneker.Stripe.PaymentMethods
```

## Configuration

```json
{
  "Stripe": {
    "SecretKey": "sk_test_..."
  }
}
```

## Usage

```csharp
using Soenneker.Stripe.Enums.PaymentMethodTypes;
using Soenneker.Stripe.PaymentMethods.Abstract;
using Soenneker.Stripe.PaymentMethods.Registrars;
using Stripe;

services.AddStripePaymentMethodsUtilAsScoped();

List<PaymentMethod>? cards = await paymentMethodsUtil.GetAllByUserIdAndType(
    "cus_...",
    StripePaymentMethodType.Card,
    cancellationToken);

PaymentMethod? defaultCard = await paymentMethodsUtil.GetFirstByUserIdAndType(
    "cus_...",
    StripePaymentMethodType.Card,
    cancellationToken);

await paymentMethodsUtil.Attach(
    "pm_...",
    new PaymentMethodAttachOptions { Customer = "cus_..." },
    cancellationToken: cancellationToken);
```

`GetAll`, `GetAllByUserIdAndType`, and `GetAllByUserIdAndTypes` follow Stripe pagination and materialize every result, so use them carefully for large accounts. Despite its historical name, `Delete` detaches the payment method from its customer; it does not erase the payment method object from Stripe.
