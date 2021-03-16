namespace KelvinTodo.Data
{
    // Repositories only responsibility should be to build aggregate roots and save aggregate roots
    // They should only handle commands.
    // We are not using them for queries, we are not using them for reads or queries or views.
    
    //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/
    public interface IAggregateRepository<T, in TKey> where T : Aggregate<TKey>
    {
        T CreateNew();
        T GetById(TKey id);
        void Save(T agg);
    }
}