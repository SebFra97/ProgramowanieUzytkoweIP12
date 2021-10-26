namespace CQRS
{
    public interface IQueryHandler<in T, out D> where T : IQuery
    {
         public D Handle(T query);
    }
}
