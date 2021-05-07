using System;
using System.Linq;
using System.Collections.Generic;
using KelvinTodo.Events;
using KelvinTodo.Commands;

namespace KelvinTodo.Data
{
    // In memory data model, the usage is an example of projection.
    // But this model could also just be used for any validation. But it should only hold what is necessary.
    class CurrentState
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }   
    }

    public class TodoDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }   
    }

    public class Todo: Aggregate<int> { 
        private CurrentState _state = new();

        public string Name => _state.Name;
        public string Description => _state.Description;
        public bool Done => _state.Done;

        public Todo(int id, IEnumerable<IEvent> events) : base(id, events)
        {
            // This is rebuilding a projection from the ground up with each constructor call
            _events.ToList().ForEach(Apply);
        }

        public void Create(CreateTodoCommand command)
        {
            var evt = new TodoCreated
            {
                Id = Id,
                Name = command.Name,
                Description = command.Description,
                Timestamp = DateTimeOffset.UtcNow,
            };
            // var evt = new TodoCreated(Id, command.Name, command.Description, DateTimeOffset.UtcNow);
            _events = _events.Append(evt);
            Apply(evt);
        }

        public void UpdateDone(SetTodoDoneCommand command)
        {
            if (_state.Done == command.Done)
                return;

            var evt = new TodoDoneUpdated
            {
                Id = Id,
                Done = command.Done,
                Timestamp = DateTimeOffset.UtcNow,
            };
            _events = _events.Append(evt);
            Apply(evt);
        }

        private void Apply(IEvent evt)
        {
            // Build up a 'current state'. Useful for validation logic
            // Current state is an example of a projection (a view based on events)
            // Right now this class is a mix of both the aggregate as well as projection.
            // Aggregate is just the stream of events (where the entity is identified by ID)
            // The current state is the projection, but we could have some other projection
            // e.g. number of times edited or something, or last edited
            
            // The projection should really be a consumer of the events and build up their own current state
            // whether that's in memory or sql or nosql...
            switch (evt)
            {
                case TodoCreated todoCreated:
                    _state.Name = todoCreated.Name;
                    _state.Description = todoCreated.Description;
                    _state.Done = false;
                    break;
                case TodoDoneUpdated todoDoneUpdated:
                    _state.Done = todoDoneUpdated.Done;
                    break;
            }
        }
    }

    public static class TodoDtoMapper
    {
        public static TodoDto ToDto(this Todo todo)
        {
            if (todo == null) return null;
            
            return new()
            {
                Id = todo.Id,
                Name = todo.Name,
                Description = todo.Description,
                Done = todo.Done,
            };   
        }
    }
}
