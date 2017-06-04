using Drawing.Models;
using System;
using System.Collections.Generic;


namespace Drawing.Services
{
    interface IStorageService
    {
        void Update(Diagram diagram);
        void Delete(Guid id);
        void Add(Diagram diagram);
        Diagram GetById(Guid id);
        Diagram GetByName(String name);
        List<Diagram> GetAll();
    }
}
