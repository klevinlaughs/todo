namespace KelvinTodo.Commands
{
    public class CreateTodoCommand 
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
    // public record CreateTodoCommand(string Name, string Description);
}
