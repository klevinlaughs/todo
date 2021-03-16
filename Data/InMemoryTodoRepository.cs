using KelvinTodo.Events;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace KelvinTodo.Data
{
    public interface ITodoRepository : IAggregateRepository<Todo, int> {}
    
    public class InMemoryTodoRepository : ITodoRepository
    {
        // In memory event store
        private readonly IDictionary<int, IEnumerable<IEvent>> _eventStore = new Dictionary<int, IEnumerable<IEvent>>();
        private int _counter;

        public Task<Todo> CreateNewAsync()
        {
            _counter++;
            var id = _counter;
            var evts = new List<IEvent>();
            return Task.FromResult(new Todo(id, evts));
        }

        public Task<Todo> GetByIdAsync(int id)
        {
            var todo = _eventStore.TryGetValue(id, out var events) ? new Todo(id, events) : null;
            return Task.FromResult(todo);
        }

        public Task SaveAsync(Todo todo)
        {
            _eventStore[todo.Id] = todo.Events;
            return Task.CompletedTask;
        }
    }
}

