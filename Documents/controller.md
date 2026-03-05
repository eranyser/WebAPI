# Controllers

#### What is a Controller

- A controller gathers together, in a simple class, all c# methods that implement a set of logically connected web  APIs. We put together things that logically belong together in a single class and we call this class a controller.
- The controller in a Web API **inherits from ControllerBase.** This marks this class to be ASP.NET Core Controller. ControllerBase is the base class for all API Controllers.
- Additionally, in order to tell ASP.NET Core that we really want to write Web API, we need to specify the attribute **[ApiController]**
  ```c#
  [ApiController]
  public class ToDoItemsController : ControllerBase
  {

  }
  ```

#### What are attributes

- It is similar to *Decorator* in javascript and *Annotation* in java 
- Attributes are passive addition to the class, they don't implement code. Can influence how code executed. They don't implement anyting. It doen's generate machind code or intermidiate language code. This attribute can be interpreted by somebody. (for example by reflection)
- Example of attribute is the one mentioned above **[ApiController]** - attribute that tells the interpreter to treat this class differently, i.e. as a controller. It is interperted at runtime, not compiled at compile time. It doesn't generate code but it influences the kind of processing of this controller class.
- Another common attributes in the controller class are **Route Attributes** which specify where the controller should be available in terms of HTTP path. For example:

  - For the whole *Controller*, we can use:
    - **[Route("[controller]")]** - Token attribute. 
      ```c#
      [ApiController]
      [Route("[controller]")]
      public class ToDoItemsController : ControllerBase
      {

      }
      ```
      When we say controller in the path we mean to use the name of the class without the suffix controller. For example, in the Class *ToDoItemController*, we can navigate to **http://localhost:5000/ToDoItems**
    - **[Route("api/todo-items")]** - We can specify our own route to the whole controller. I.e we can navigate to **http://localhost:5000/api/todo-items**
      ```c#
      [ApiController]
      [Route("api/todo-items")]
      public class ToDoItemsController : ControllerBase
      {

      }
      ```

  - For Action, we can also add routes:
    - **[Route("item")]** - we need a route also to the action.
    - **[Route("[action]")]**  - Same we can specify the action keyword for the methods.
    - **[Route("[controller]/[action]")]** - or we can combine theme togeter

- [Pay attention to diference between Attributes and Filters](AttributesVSFilters.md)

- Before continue adding new APIs, there is an article related to [Choosing between controller-based APIs and minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0)

- Regarding *Routing Rules* you can read more [here](routing.md)

- Lets add some APIs:
  ```c#
  namespace ToDoItems.Controllers
  {
    [ApiController]
    [Route("api/todo-items")]
    public class ToDoItemsController : ControllerBase
    {
      private static readonly List<string> items =
        new List<string> { "Clean my room", "Feed the cat" };

      [HttpGet]
      public IActionResult GetAllItems()
      {
        return Ok(items);
      }
    }
  }
  ```

  - First we add a list of *items*. it is ***private static readonly*** which make it:
    -  **private**: available / accessible only from this class
    -  **static**: Make no need to create an instance of *ToDoItemsControler* class to access the list. There is a single *items* list for **all** *TodoItemsController*'s. 
       
       **An important thing to understand is that we have many TodoItemsController's. For any HTTP request we get a new TodoItemsController, and the list of items should be kind of a database. In our case it's just in-memory, and the *static* causes the list to be only one for all *TodoItemsController's***
    - **readonly**: The value of the list can be assigned only once, and can't be change anymore.

  - We want to make the *itmes* list availabe to the WebAPI, so we add a function *GetAllItems*, adding the attribute **[HttpGet]**, and return the http status code, **Ok** in our case, and give it the content of the result.
  - The return type is *IActionResult*. which is encapsulation of status code and content.
  - To get the *items* List according current *routing rules* we use: https://localhost:5000/api/todo-items

- Let's add the method to get an item by its *index*
  ```c#
  [HttpGet]
  [Route("{index}", Name = "GetSpecificItem")]
  public IActionResult GetItem(int index)
  {
    if (index >= 0 && index < items.Count)
    {
      return Ok(items[index]);
    }
    return BadRequest("Invalid index");
  }
  ```
    - In the **Route** attribute there is a curly braces. Theas means that part of the route is a **variable** that refers the first parameter of the method. To get a speicific item we can use: https://localhost:5000/api/todo-items/1. This will return the item with index "1". We will talk about it in the [routing rules](routing.md) page.

- Now lets add the method *AddItem*

    ```c#
		[HttpPost]
		public IActionResult AddItem([FromBody] string newItem)
		{
			items.Add(newItem);
			return CreatedAtRoute("GetSpecificItem", new { index = items.IndexOf(newItem) }, newItem);
		}
    ```
    - We set the **[HttpPost]** attribute, it is an *insert*. We add a new item string.
    - In addition we also *decorate* the parameter with attribute **[FromBody]**. This means, takes the new string item from the **HTTP Body**
    - We return value using the *CreatedAtRoute* mehtod. It is good practice to return the *location* header, so we will be able to get the newly created string item. The first paremeter to the method is the *Name* atribute at the method that brings the item, so we have to add *Name* to the *GetItem* method.
    - We actually say, there is a **Route**, named **GetSpecificItem** that we can query the new added item, giving its index for this route. The last parameter will be returned as a response body. Usually, it is just the value of newly created resource.
    - **More reading and explanations:** [Created, CreatedAtAction, CreatedAtRoute Methods In ASP.NET Core Explained With Examples](https://ochzhen.com/blog/created-createdataction-createdatroute-methods-explained-aspnet-core)

- Lets add the *UpdateItem* method
    ```c#
    [HttpPut]
    [Route("{index}")]
    public IActionResult UpdateItem(int index, [FromBody] string newItem)
    {
      if (index >= 0 && index < items.Count)
      {
        items[index] = newItem;
        return Ok();
      }
      return BadRequest("Invalid index");
    }
    ```

    - In this method we use the **[HttpPut]** attribute
    - We are combining the *Route* parameter, **id**, with the data from **[FromBody]** attribute. This is because we want to update specific item.
  
- *DeleteItem* will look like this:

    ```c#
    [HttpDelete] 
    [Route("{index}")]
    public IActionResult DeleteItem(int index)
    {
      if (index >= 0 && index < items.Count)
      {
        items.RemoveAt(index);
        return NoContent();
      }

      return BadRequest("Invalid index");
    }
    ```
- As we saw the **[FromBody]** attribute that tells .NET that the data comes from the body of the requeset, we can set the attribute **[From Query]** to indicate, for a parameter, that it comes from the query string. A query string comes after a question mark.
  - In the next method we request a sorted list of items, we set the *routing rules* to be **sorted** and add a *query string* parameter - *sortOrder* that can be **'asc' or 'desc'**. To get a sorted item list we can use: https://localhost:5000/api/todo-items/sorted?sortOrder=desc

    ```c#
    [HttpGet]
    [Route("sorted")]
    public IActionResult GetAllItemsSorted([FromQuery] string sortOrder)
    {
      return sortOrder switch
      {
        "desc" => Ok(items.OrderByDescending(item => item)),
        "asc" => Ok(items.OrderBy(item => item)),
        _ => BadRequest("Invalid or missing sortOrder query parameter")
      };
    }
    ```

[Back to Table of Content](../README.md#02-webapi-basic-conceptes)

**Bibliography:**

[![Watch the video](https://i.ytimg.com/vi/-O0UYM0ZIIc/hqdefault.jpg?sqp=-oaymwEbCKgBEF5IVfKriqkDDggBFQAAiEIYAXABwAEG&rs=AOn4CLDbNRYNMEmt4sGKqGGZJGzFsrrmNQ)](https://www.youtube.com/watch?v=-O0UYM0ZIIc&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=20&pp=iAQB)

[HTL Perg: Mobile Computing and C# Course - Part 2 (ASP.NET Core Fundamentals) **44:36**](https://www.youtube.com/watch?v=SpXNoqPJDwU&list=PLhGL9p3BWHwtV_hn6H_uZ4vrFE3F7mY8a&index=2&t=2725s&pp=iAQB)

[![Watch the video](https://i.ytimg.com/vi/SpXNoqPJDwU/hqdefault.jpg?sqp=-oaymwEbCKgBEF5IVfKriqkDDggBFQAAiEIYAXABwAEG&rs=AOn4CLAnVeJkF4Vor0M6vFNAKMGSiPBG6Q)](https://www.youtube.com/watch?v=SpXNoqPJDwU&list=PLhGL9p3BWHwtV_hn6H_uZ4vrFE3F7mY8a&index=2&t=2725s&pp=iAQB)

[C# ASP.NET 5 - Dependency Injection, RESTful Web APIs, Generating Swagger **18:48**](https://www.youtube.com/watch?v=ksy8LK5M1Ts&list=PLhGL9p3BWHwtHPWX8g7yJFQvICdNhFQV7&index=26&t=1128s&pp=iAQB)

[![More on Controllers](https://i.ytimg.com/vi/ksy8LK5M1Ts/hqdefault.jpg?sqp=-oaymwEbCKgBEF5IVfKriqkDDggBFQAAiEIYAXABwAEG&rs=AOn4CLB_KTRMIYHfCNICxOCNUfbHxH9IvQ)](https://www.youtube.com/watch?v=ksy8LK5M1Ts&list=PLhGL9p3BWHwtHPWX8g7yJFQvICdNhFQV7&index=26&t=1128s&pp=iAQB)

- The sides for this are taken from the mobile course:
  - [htl-mobile-computing-5](https://github.com/rstropek/htl-mobile-computing-5) given in 2019/20, [slides](https://htl-mobile-computing-5.azurewebsites.net/#/)
  - [htl-mobile-computing](https://github.com/rstropek/htl-mobile-computing) given in 2018/19, [slides](https://rstropek.github.io/htl-mobile-computing/#/). This is an older version of the same course, includes lot of issues that are missing in the first github repository.
