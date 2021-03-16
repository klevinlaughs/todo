using System.Collections.Generic;
using System.Linq;
using KelvinTodo.Events;

namespace KelvinTodo.Data
{
    public abstract class Aggregate<TKey>
    {
        /**
         * Unique identifier per stream
         */
        public TKey Id { get; set; }
        // Events
        protected IEnumerable<IEvent> _events { get; set; } = new List<IEvent>();
        public IReadOnlyList<IEvent> Events => _events.ToList().AsReadOnly();

        // Apply Events
    }
}