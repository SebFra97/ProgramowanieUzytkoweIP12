using Models.DTO;
using System.Collections.Generic;

namespace RepositoryPattern
{
    public interface IAuthorRepository
    {
        List<AuthorDto> GetAllAuthors();
        void CreateNewAuthor(AuthorDto newAuthor);
        bool DeleteAuthor(int id);
        void AddRateToAuthor(int id, int rate);
    }
}
