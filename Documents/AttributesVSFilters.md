# What is the difference between Attribute and Filter in .net WebAPI

[From CahtGPT](https://chat.openai.com/c/7d40e0ed-be4f-4f0c-89a6-32a213ae2fc5)  

In the context of .NET WebAPI, the terms "Attribute" and "Filter" are often related to aspects of customization and behavior in the processing pipeline of HTTP requests and responses. Let's discuss each concept separately:

**Attribute:**
- **Definition:** In .NET WebAPI, attributes are used to add metadata to code elements like classes, methods, or properties. They provide a way to declaratively add information to the code, and this information is then used by the runtime or other components to modify behavior.
- **Example: HttpGet, HttpPost**, and **Route** are examples of attributes in WebAPI. They are used to define the HTTP method and route for an API endpoint. 
  ```c#
  [HttpGet]
  [Route("api/sample")]
  public IHttpActionResult GetSampleData()
  {
    // Implementation
  }
  ```
**Filter:**
- **Definition:** Filters in .NET WebAPI are components that allow you to implement cross-cutting concerns such as logging, exception handling, authentication, and authorization. They are applied to the HTTP request and response processing pipeline.
- **Types:** There are various types of filters, including Action Filters, Exception Filters, Authorization Filters, and Result Filters.
- **Example:** An action filter is often used to perform some logic before or after the execution of an action method.
  ```c#
  public class LogActionFilterAttribute : ActionFilterAttribute
  {
      public override void OnActionExecuting(HttpActionContext actionContext)
      {
          // Logic executed before the action method is called
          Log("Action is executing");
      }
  
      public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
      {
          // Logic executed after the action method is called
          Log("Action has been executed");
      }
  
      private void Log(string message)
      {
          // Log the message
      }
  }
  ```
- **Usage:**
  ```c#
  [LogActionFilter]
  public IHttpActionResult GetSampleData()
  {
      // Implementation
  }
  ```  
In summary, attributes are used for declarative metadata, while filters are used to inject logic into the processing pipeline of HTTP requests and responses in a more procedural manner. Attributes are often applied to define the characteristics of API elements, while filters are applied to influence the behavior of the request/response processing pipeline.
