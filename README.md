# ODataEcom - OData Sample code in ASPNETCORE

ODataEcom is a example solution, built to demonstrate OData API implementation in an ASP.NET Core (.NET 5) API. I have tried to cover multiple scenarios but feel free to create issues if I have misssed the one you needed.

# What is OData?

OData stands for Open Data. It is an ISO/IEC approved, OASIS standard that defines a set of best practices for building and consuming RESTful APIs. It can help enhance an API to have extensive capabilities by itself, while we don't need to worry much about the data processing and response transformations as a whole and instead concentrate only on building the business logic for the API. OData adds one layer over the API treating the endpoint itself as a resource and adds the transformation capabilities via the URL.

One can integrate the prowess of OData into an ASP.NET Core API by installing the OData nuget available for .NET Core and get started.

# Technologies

1. ASP.NET Core (.NET 5)
2. Entity Framework Core (EF Core 5)
3. OData Library for ASP.NET Core (8.0.4)
4. SQLite

# About the Boilerplate

This boilerplate is a perfect starter for developers looking to implement OData. The solution offers the following:

1. EFCore with SQLite
2. OData controller to perform CRUD operation
3. Adding Authentication and Authorization- in pipeline


# Getting Started

To get started, follow the below steps:

1. Install .NET 5 SDK
2. Clone the Solution into your Local Directory
3. Navigate to the Cloned directory and run the solution
4. Adding swagger to allow test the endpoint

# Testing the Solution

The solution contains necessary seeding code and uses SQLite database, so once the solution starts you'd already have data ready.

Once the solution is running, open a browser and try the below URLs to see OData in action. 

# To view the Metadata:

```
https://localhost:44311/v1/$metadata
https://localhost:44311/v2/$metadata
```

# Simple Get:

```
https://localhost:44311/v1/products
```

# Get with OData Queries:

```
https://localhost:44311/v1/product?$filter=Id gt 100&$select=Id,Name&$skip=100&$top=100
```

# Get Count:

```
https://localhost:44311/v1/product/$count
```

# Get by Id:

```
https://localhost:44311/v1/product(1500)
https://localhost:44311/v1/ProductCategory?$expand=Products($filter=Id eq 10)
```
https://localhost:44311/v2/Product?$orderby=Id desc, Name desc&$expand=ProductCategory($select=Name;$orderby=Name)
The blog is available at:

https://medium.com/@nabinkjha/image-source-f258c5aedc33

Leave a Star if you find the solution useful. It inspire me to contribute more

<a href="https://www.buymeacoffee.com/nabinjha" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
