using RepositoryPattern;
using System.Collections.Generic;

namespace CQRS
{
    public interface IAuthorQueryRepository
    {
        List<AuthorDto> GetAllAuthors();
    }
}
