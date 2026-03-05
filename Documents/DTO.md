# DTO - Data Transfer Object

#### DTO

What we saw till now, (described [here](./controller.md)), was little bit trivial, cause what we did was maintaining a list of strings. In practice we don't maintain a list of strings or ints or dates. Typically we maintain a list of business objects, like *Customer*. *Order* etc.

So let's add a DTO Class. (we will copy it in front of *ToDoItemController*)
  ```c#
  public class TodoItem
  {
      [MinLength(5)]
      [MaxLength(50)]
      [Required]
      public string Description { get; set; }
  
      [MaxLength(50)]
      public string AssignedTo { get; set; }
  }
  ```
We have also added data annotation to the properties in this DTO. We can define some business rules, for example, what fiedls are mandatory, nimimum and maximum length, etc.
The great thing about it, is that we don't have to check that. ASP.NET Core does this automatically. If someone sends a *TodoItme* without a description ASP.NET Core will recongnize the *required* attribute and it will return an error. 

The changes we will need to do:

- Change the collection from a list of strings to a list of *TodoItem*
  ```c#
  private static readonly List<TodoItem> items =
    new List<TodoItem> {
        new TodoItem { Description ="Clean my room", AssignedTo = "John Dow" },
        new TodoItem { Description ="Feed the cat", AssignedTo = "Me" }
    };
  ```
- Change the *AddItem* method to get in its parameter a *TodoItem* instead of a *string*
  ```c#
  [HttpPost]
  public IActionResult AddItem([FromBody] TodoItem newItem)
  {
    items.Add(newItem);
    return CreatedAtRoute("GetSpecificItem", new { index = items.IndexOf(newItem) }, newItem);
  }
  ```
  We get here a complete TodoItem. ASP.NET transform it to *TodoItem* DTO. All the sanity check, like:
    - Is it JSON?
    - Does it have a description?
    - Does the description have the required length?

  are all done Automatically. This is called [***Model Validation***](https://learn.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api)


[Back to Table of Content](../README.md##Branches)

**Bibliography:**

[HTL Perg: Mobile Computing and C# Course - Part 2 (ASP.NET Core Fundamentals) **1:10:57**](https://www.youtube.com/watch?v=SpXNoqPJDwU&list=PLhGL9p3BWHwtV_hn6H_uZ4vrFE3F7mY8a&index=2&t=2725s&pp=iAQB)

[![Watch the video](https://i.ytimg.com/vi/SpXNoqPJDwU/hqdefault.jpg?sqp=-oaymwEbCKgBEF5IVfKriqkDDggBFQAAiEIYAXABwAEG&rs=AOn4CLAnVeJkF4Vor0M6vFNAKMGSiPBG6Q)](https://www.youtube.com/watch?v=SpXNoqPJDwU&list=PLhGL9p3BWHwtV_hn6H_uZ4vrFE3F7mY8a&index=2&t=2725s&pp=iAQB)