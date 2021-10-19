using Models.DTO;
using System.Collections.Generic;

namespace CQRS
{
    public interface IAuthorQueryRepository
    {
        List<AuthorDto> GetAllAuthors();
    }
}
