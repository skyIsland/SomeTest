using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[todo]")]
    public class TodoController : Controller
    {
        public ITodoRepository TodoItems { get; set; }

        public TodoController(ITodoRepository todoItems)
        {
            TodoItems = todoItems;
        }
        /// <summary>
        /// 获取所有的任务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TodoItem> GetAll()
        {
            return TodoItems.GetAll();
        }

        [HttpGet("{id}",Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var item = TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
    }
}