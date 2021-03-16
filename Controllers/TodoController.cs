using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KelvinTodo.Commands;
using KelvinTodo.Data;

namespace KelvinTodo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly TodoRepository _todoRepository;

        public TodoController(
            ILogger<TodoController> logger,
            TodoRepository todoRepository
            )
        {
            _logger = logger;
            _todoRepository = todoRepository;
        }

        [HttpGet("{id}")]
        public TodoDto GetById([FromRoute] int id)
        {
            var todoDto = _todoRepository.GetById(id).ToDto();
            return todoDto;
        }

        [HttpPost]
        public TodoDto Create([FromBody] CreateTodoCommand command)
        {
            // TODO: send to a mediatr or something, signalr?
            // There should be a command handler
            var todo = _todoRepository.GetDefault();
            todo.Create(command);
            _todoRepository.Save(todo);
            return todo.ToDto();
        }

        [HttpPut("{id}/done")]
        public ActionResult<TodoDto> ToggleDone([FromRoute] int id, [FromBody] ToggleTodoDoneCommand command)
        {
            var todo = _todoRepository.GetById(id);
            if (todo == null)
                return NotFound();

            todo.UpdateDone(command);
            _todoRepository.Save(todo);
            return Ok(todo.ToDto());
        }

        [HttpDelete("{id}")] 
        public void Delete()
        {
        }
    }
}
