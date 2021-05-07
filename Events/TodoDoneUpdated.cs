using System;

namespace KelvinTodo.Events
{
    // public record TodoCreated(string Id, string Name, string Description, bool Done, DateTimeOffset timestamp) : IEvent;
    [Serializable]
    public class TodoDoneUpdated : IEvent {
        public int Id { get; set; }
        public bool Done { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int Version => 1;
    }
}
