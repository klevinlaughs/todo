using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KelvinTodo.Commands;
using KelvinTodo.Data;
using MediatR;

namespace KelvinTodo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoRepository _todoRepository;
        private readonly IMediator _mediator;

        public TodoController(
            ILogger<TodoController> logger,
            ITodoRepository todoRepository,
            IMediator mediator
            )
        {
            _logger = logger;
            _todoRepository = todoRepository;
            _mediator = mediator;
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
            var todo = await _mediator.Send(command, cancellationToken);
            
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
