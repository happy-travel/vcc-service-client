# vcc-service-client
Client for VCC Service (Virtual credit card issuer)

### Description
Client implemented service for issuing virtual credit cards in VCC service

### Options
- VccEndpoint - endpoint for VCC service (**required**)
- IdentityEndpoint - token endpoint for IdentityServer (**required**)
- IdentityClient - IdentityServer client with 'vcc_service' scope (**required**)
- IdentitySecret - IdentityServer client's password (**required**)
- RetryPolicy - delegate which returns IAsyncPolicy<HttpResponseMessage> (**optional**)

### Usage
#### Configure client
```c#
using HappyTravel.VccServiceClient.Extensions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services..AddVccService(options => {
            options.VccEndpoint = "api endpoint for VCC service",
            options.IdentityEndpoint = "token endpoint for IdentityServer",
            options.IdentityClient = "client",
            options.IdentitySecret = "password"
        })
        ...
    }
}
```
#### Inject service and use
```c#
using HappyTravel.VccServiceClient.Models;
using HappyTravel.VccServiceClient.Services;

public class SomeService
{
    public SomeService(IVccService vccService)
    {
        _vccService = vccService;
    }
    
    public async Task SomeMethod()
    {
        var (_, isFailure, virtualCreditCard, error) = await _vccService.IssueVirtualCreditCard(referenceCode, moneyAmount, activationDate, dueDate);
    }
}
```
