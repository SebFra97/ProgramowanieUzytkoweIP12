using Nest;
using System;

namespace Model
{
    public class Repo
    {
        public IElasticClient elasticClient { get; set; }

        public Repo(string url)
        {
            elasticClient = new ElasticClient(new ElasticConnection(new Uri(url)));
        }
    }
}
