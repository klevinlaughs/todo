using System;

namespace KelvinTodo.Events
{
    // public record TodoCreated(string Id, string Name, string Description, bool Done, DateTimeOffset timestampe) : IEvent;
    [Serializable]
    public class TodoDoneUpdated : IEvent {
        public int Id { get; set; }
        public bool Done { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
