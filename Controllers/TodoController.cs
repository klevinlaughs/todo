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
        private readonly ITodoRepository _todoRepository;

        public TodoController(
            ILogger<TodoController> logger,
            ITodoRepository todoRepository
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
            // There would eventually be some consumer of the created event
            // One of those would be a "TodoProjection".
            
            // TODO: who is responsible for generating IDs? it depends, but maybe the more correct approach is for the
            // client to send it. Some discussion: https://github.com/gregoryyoung/m-r/issues/17
            // https://stackoverflow.com/questions/43433318/cqrs-command-return-values
            var todo = _todoRepository.CreateNew();
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
            // TODO: how to delete in CQRS
            throw new NotImplementedException();
        }
    }
}
