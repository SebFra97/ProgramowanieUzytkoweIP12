using RepositoryPattern;

namespace CQRS
{
    public interface IAuthorCommandRepository
    {
        void CreateNewAuthor(AuthorDto newAuthor);
        bool DeleteAuthor(int id);
        void AddRateToAuthor(int id, int rate);
    }
}
