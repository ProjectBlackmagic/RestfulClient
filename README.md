# RestfulClient [![Build Status](https://travis-ci.org/ProjectBlackmagic/RestfulClient.svg?branch=master)](https://travis-ci.org/ProjectBlackmagic/RestfulClient)

## How to use

```csharp
// It is very easy...
// ... just create a new client,
var client = new RestfulClient("https://jsonplaceholder.typicode.com/");

// execute the request
var result = client.Get<List<BlogPost>>("posts");

// and th e response body will be desirialized as an object of specified type:
public class BlogPost {
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}

```

## Nuget packages distribution

## Supported Authorization providers

We expose an interface ```IAuthenticator``` and a generic ```AuthClient``` to define how to use different authentication providers. Once the client is created, it is used the same as the ```RestfulClient```.

```csharp
// Create your authenticator
IAuthenticator authenticator = ...;

// Pass the authenticator to the AuthClient
var client = new AuthClient(authenticator, "http://yoururlhere.com");

// Execute your request(s)
var response = client.Get<MyObject>("resource");
```


### Azure Active Directory

For AAD authentication, we already define both a configuration interface ```IAadConfig``` and authenticator ```AadAuthenticator```.

```csharp
IAadConfig config = new AadConfig(...);
IAuthenticator authenticator = new AadAuthenticator(config);

var client = new AuthClient(authenticator);

...
```

### RPS (Live ID)

For RPS (Live ID) authentication, we already define both a configuration interface ```IRpsConfig``` and authenticator ```RpsAuthenticator```.

```csharp
IRpsConfig config = new RpsConfig(...);
IAuthenticator authenticator = new RpsAuthenticator(config);

var client = new AuthClient(authenticator);

...
```