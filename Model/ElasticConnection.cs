using Models.DTO;
using Nest;
using System;

namespace Model
{
    public class ElasticConnection : ConnectionSettings
    {
        public ElasticConnection(Uri uri = null) : base(uri)
        {
            this.DefaultMappingFor<BookDto>(x => x.IndexName("books_index"));
        }
    }
}
