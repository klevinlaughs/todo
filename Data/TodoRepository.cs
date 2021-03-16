using KelvinTodo.Events;
using System.Collections.Generic;
using System;

namespace KelvinTodo.Data
{
    // Repositories only responsibility should be to build aggregate roots and save aggregate roots
    // They should only handle commands.
    // We are not using them for queries, we are not using them for reads or queries or views.
    public class TodoRepository
    {
        private IDictionary<int, IEnumerable<IEvent>> _eventStore = new Dictionary<int, IEnumerable<IEvent>>();
        private int counter = 0;

        public Todo GetDefault()
        {
            counter++;
            var id = counter;
            var evts = new List<IEvent>();
            return new Todo(id, evts);
        }

        public Todo GetById(int id)
        {
            if (_eventStore.TryGetValue(id, out var evts)) {
                return new Todo(id, evts);
            };

            return null;
        }

        public void Save(Todo todo)
        {
            _eventStore[todo.Id] = todo.Events;
        }
    }
}

