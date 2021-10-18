﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    public interface IAuthorRepository : IDisposable
    {
        List<AuthorDto> GetAllAuthors();
        void CreateNewAuthor(Author author);
        void DeleteAuthor(int id);
        void AddRateToAuthor(int id, int rate);
    }
}
