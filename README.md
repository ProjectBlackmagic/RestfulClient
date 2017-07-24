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

### Azure Active Directory

### RPS (Live ID)