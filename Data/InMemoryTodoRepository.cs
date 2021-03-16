using KelvinTodo.Events;
using System.Collections.Generic;
using System;

namespace KelvinTodo.Data
{
    public interface ITodoRepository : IAggregateRepository<Todo, int> {}
    
    public class InMemoryTodoRepository : ITodoRepository
    {
        // In memory event store
        private readonly IDictionary<int, IEnumerable<IEvent>> _eventStore = new Dictionary<int, IEnumerable<IEvent>>();
        private int _counter;

        public Todo CreateNew()
        {
            _counter++;
            var id = _counter;
            var evts = new List<IEvent>();
            return new Todo(id, evts);
        }

        public Todo GetById(int id)
        {
            return _eventStore.TryGetValue(id, out var events) ? new Todo(id, events) : null;
        }

        public void Save(Todo todo)
        {
            _eventStore[todo.Id] = todo.Events;
        }
    }
}

