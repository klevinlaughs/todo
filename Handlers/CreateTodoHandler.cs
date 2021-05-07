using System.Threading;
using System.Threading.Tasks;
using KelvinTodo.Commands;
using KelvinTodo.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KelvinTodo.Handlers
{
    public class CreateTodoHandler: IRequestHandler<CreateTodoCommand, Todo>
    {
        private readonly ILogger<CreateTodoHandler> _logger;
        private readonly ITodoRepository _todoRepository;

        public CreateTodoHandler(
            ILogger<CreateTodoHandler> logger,
            ITodoRepository todoRepository)
        {
            _logger = logger;
            _todoRepository = todoRepository;
        }
        
        public async Task<Todo> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            // TODO: who is responsible for generating IDs? it depends, but maybe the more correct approach is for the
            // client to send it. Some discussion: https://github.com/gregoryyoung/m-r/issues/17
            // https://stackoverflow.com/questions/43433318/cqrs-command-return-values
            var todo = await _todoRepository.CreateNewAsync(cancellationToken);
            todo.Create(request);
            await _todoRepository.SaveAsync(todo);
            
            // Should commands care about results? Or just success/fail?
            return todo;
        }
    }
}
