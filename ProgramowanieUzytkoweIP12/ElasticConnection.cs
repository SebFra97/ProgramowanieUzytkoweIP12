using Models.DTO;
using Nest;
using System;

namespace ProgramowanieUzytkoweIP12
{
    public class ElasticConnection : ConnectionSettings
    {
        public ElasticConnection(Uri uri = null) : base(uri)
        {
            this.DefaultMappingFor<BookDto>(x => x.IndexName("books.index"));
            this.DefaultMappingFor<AuthorDto>(x => x.IndexName("authors.index"));
        }
    }
}
