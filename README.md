# AutoProc
AutoProc is a lightweight Asp.Net Core 2 middleware package which makes it easy to create a backend by exposing your stored procedures as an API.

# Please, understand this project is in early days. 
# DO NOT USE IN PRODUCTION YET.

## Basic Usage
With a couple of lines of code you can start having a working app without the hazle of coding controllers for each api, or changing query statements in your app. This way all your business logic resides in the database and delutes the backend size by a big margin.

### In your backend
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  // Having just that line, AutoProc will try to get by default an IDbConnection from the IServiceProvider if your application.
  app.UseAutoProc();
}
```

### in javascript
```js
fetch('api/autoproc/myapp/dbo/GET/Items')
    .then(x => x.json())
    .then(items => console.log('items', items));
```

This will execute the Following Stored Procedure:  [dbo].[P_GET_Items] 

As you can see, only P_X_Y named stored procedures can be executed. This will prevent execution of those not planned to.

`myapp` in the url is just a string that is passed around and can be useful specially when dealing with multiple database apps. Data is passed by posting a body or via querystring.


## Options

### Path
Simply sets where the routing is going to start the begining of the mapping.
Defaults to  `api/autoproc`

### OnNeedDbConnection
This is called before every execution. You can change the connection dynamically this way As an example we get it from IServiceProvider, but you may create a new connection based on the parameters passed. 

`context` is the HttpContext 

`aprRequest` contains parsed information about from the request and query
 
### BypassORM
Defaults to false. Setting this to true makes AutoProc look for the the first column which the name starts with "JSON" and returns the string content from the first row.

### Example:
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  app.UseAutoProc(options=> 
  { 
      options.OnNeedDbConnection = (context, aprequest) => 
      {
          if (aprequest.Procedure == "ImportantCentralizedSP")
            return new SqlConnnection("CONNECTION STRING A ")
          else
            return new SqlConnnection("CONNECTION STRING B ")
      };
      options.BypassORM = true; //My SP returns valid JSON
  });
}
```

## AutoProcRequest
### TODO

## How to pass parameters
### TODO
