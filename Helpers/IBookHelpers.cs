using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helpers
{
    public interface IBookHelpers
    {
        public Task<List<AuthorVM>> GetAuthorsOfBook(Book inputBook);
        public string CountBookAverageRate(List<BookRate> inputData);
        public int CountBookRates(Book inputBook);
    }
}
