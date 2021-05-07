using KelvinTodo.Data;
using MediatR;

namespace KelvinTodo.Commands
{
    public class CreateTodoCommand : IRequest<Todo>
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
    // public record CreateTodoCommand(string Name, string Description);
}
