using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        [HttpGet("{id:int}")]
        public async Task<TodoDto> GetById([FromRoute] int id)
        {
            // TODO: this should come from a query model?
            var todo = await _todoRepository.GetByIdAsync(id);
            return todo.ToDto();
        }

        [HttpPost]
        public async Task<TodoDto> Create([FromBody] CreateTodoCommand command, CancellationToken cancellationToken)
        {
            // TODO: send to a mediatr or something, signalr?
            // There should be a command handler
            // There would eventually be some consumer of the created event
            // One of those would be a "TodoProjection".
            
            // TODO: who is responsible for generating IDs? it depends, but maybe the more correct approach is for the
            // client to send it. Some discussion: https://github.com/gregoryyoung/m-r/issues/17
            // https://stackoverflow.com/questions/43433318/cqrs-command-return-values
            var todo = await _todoRepository.CreateNewAsync(cancellationToken);
            todo.Create(command);
            await _todoRepository.SaveAsync(todo);
            return todo.ToDto();
        }

        [HttpPut("{id:int}/done")]
        public async Task<ActionResult<TodoDto>> ToggleDone([FromRoute] int id, [FromBody] SetTodoDoneCommand command)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo is null)
                return NotFound();

            todo.UpdateDone(command);
            await _todoRepository.SaveAsync(todo);
            return Ok(todo.ToDto());
        }

        [HttpDelete("{id:int}")] 
        public void Delete(int id)
        {
            // TODO: how to delete in CQRS
            throw new NotImplementedException();
        }
    }
}
