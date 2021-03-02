using System;

namespace KelvinTodo.Events
{
    // public record TodoCreated(int Id, string Name, string Description, DateTimeOffset Timestamp) : IEvent;
    public record TodoCreated : IEvent {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTimeOffset Timestamp { get; init; }
    }
}
