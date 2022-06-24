using Bookstore.Microservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Shared.Interfaces
{
    public interface IBookService
    {
        Task<BookModel> AddBookService(BookModel newBookDetails);
        Task<List<BookModel>> UpdateBookService(string BookID, BookModel changedBookDetails);
        Task DeleteBookService(string BookID);
        Task<BookModel> GetBookService(string BookID);
        Task<List<BookModel>> GetAllBooksService();
    }
}
