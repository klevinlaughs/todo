using System.Collections.Generic;
using System.Linq;
using KelvinTodo.Events;

namespace KelvinTodo.Data
{
    public abstract class Aggregate<TKey> where TKey : struct
    {
        /**
         * Unique identifier per stream
         */
        public TKey Id { get; }
        
        protected IEnumerable<IEvent> _events { get; set; }
        /**
         * Events
         */
        public IReadOnlyList<IEvent> Events => _events.ToList().AsReadOnly();

        protected Aggregate(TKey id, IEnumerable<IEvent> events)
        {
            Id = id;
            _events = events;
        }
            
        // Apply Events
    }
}