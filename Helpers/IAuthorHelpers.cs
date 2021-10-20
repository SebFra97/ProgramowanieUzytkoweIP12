using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helpers
{
    public interface IAuthorHelpers
    {
        public Task<List<BookVM>> GetBooksOfAuthor(Author inputAuthor);
        public string CountAuthorRateAverage(List<AuthorRate> inputData);
        public int CountAuthorRates(Author inputAuthor);
    }
}
