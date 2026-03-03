using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoItems.Controllers
{
	public class TodoItem
	{
		[MinLength(5)]
		[MaxLength(50)]
		[Required]
		public string Description { get; set; }

		[MaxLength(50)]
		public string AssignedTo { get; set; }
	}

	[ApiController]
	[Route("api/todo-items")]
	public class ToDoItemsController : ControllerBase
	{

		//	private static readonly List<string> items =
		//		new List<string> { "Clean my room", "Feed the cat" };

		private static readonly List<TodoItem> items =
			new List<TodoItem> {
				new TodoItem { Description ="Clean my room", AssignedTo = "John Dow" },
				new TodoItem { Description ="Feed the cat", AssignedTo = "Me" }
			};

		[HttpGet]
		public IActionResult GetAllItems()
		{
			return Ok(items);
		}

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

		[HttpPost]
		public IActionResult AddItem([FromBody] TodoItem newItem)
		{
			items.Add(newItem);
			return CreatedAtRoute("GetSpecificItem", new { index = items.IndexOf(newItem) }, newItem);
		}

		[HttpPut]
		[Route("{index}")]
		public IActionResult UpdateItem(int index, [FromBody] TodoItem newItem)
		{
			if (index >= 0 && index < items.Count)
			{
				items[index] = newItem;
				return Ok();
			}

			return BadRequest("Invalid index");
		}

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

		[HttpGet]
		[Route("sorted")]
		public IActionResult GetAllItemsSorted([FromQuery] string sortOrder)
		{
			return sortOrder switch
			{
				"desc" => Ok(items.OrderByDescending(item => item.AssignedTo)),
				"asc" => Ok(items.OrderBy(item => item.AssignedTo)),
				_ => BadRequest("Invalid or missing sortOrder query parameter")
			};
		}
	}
}
