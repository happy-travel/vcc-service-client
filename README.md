# gifu-client
Client for Gifu Service (Virtual credit card issuer)

### Description
Client implemented service for issuing virtual credit cards in Gifu service

### Options
- GifuEndpoint - endpoint for Gifu service
- IdentityEndpoint - token endpoint for IdentityServer
- IdentityClient - IdentityServer client with 'vcc_service' scope
- IdentitySecret - IdentityServer client's password

### Usage
#### Configure client
```c#
using HappyTravel.GifuClient.Extensions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services..AddGifuService(options => {
            options.GifuEndpoint = "api endpoint for Gifu service",
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
using HappyTravel.GifuClient.Models;
using HappyTravel.GifuClient.Services;

public class SomeService
{
    public SomeService(GifuService gifuService)
    {
        _gifuService = gifuService;
    }
    
    public async Task SomeMethod()
    {
        var (_, isFailure, virtualCreditCard, error) = await _gifuService.IssueVirtualCreditCard(referenceCode, moneyAmount, dueDate);
    }
}
```
