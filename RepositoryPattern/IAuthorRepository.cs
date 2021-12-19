using Model;
using Models.DTO;
using System.Collections.Generic;

namespace RepositoryPattern
{
    public interface IAuthorRepository
    {
        List<AuthorDto> GetAllAuthors(PaginationDto pagination);

        AuthorDto GetAuthor(int id);
        int CreateNewAuthor(AuthorDto newAuthor);
        bool DeleteAuthor(int id);
        void AddRateToAuthor(int id, int rate);
    }
}
