using System;
using System.Linq;
using System.Collections.Generic;
using KelvinTodo.Events;
using KelvinTodo.Commands;

namespace KelvinTodo.Data
{
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

    // [Serializable]
    public class Todo { 
        public int Id { get; }
        private IEnumerable<IEvent> _events { get; set; }
        private CurrentState _state = new();

        public string Name => _state.Name;
        public string Description => _state.Description;
        public bool Done => _state.Done;

        public IList<IEvent> Events => _events.ToList().AsReadOnly();

        public Todo(int id, IEnumerable<IEvent> events)
        {
            Id = id;
            _events = events;
            _events.ToList().ForEach(evt => Apply(evt));
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

        public void UpdateDone(ToggleTodoDoneCommand command)
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

        public TodoDto ToDto()
        {
            return new TodoDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Done = Done,
            };
        }
    }
}
