using Bookstore.Microservices.Models;
using Bookstore.Microservices.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Shared.Services
{
    public class BookService : IBookService
    {
        private readonly ISettingServices _settingServices;

        // Cosmos DocDB API database
        private string _docDbEndpointUri;
        private string _docDbPrimaryKey;
        private string _docDbDatabaseName;

        // Doc DB Collections
        private string _docDbDigitalMainCollectionName;

        private static CosmosClient _docDbSingletonClient;
        private readonly Container _cosmosContainer;

        public BookService(ISettingServices settingServices, IJWTService jWTService)
        {
            _settingServices = settingServices;

            _docDbEndpointUri = _settingServices.GetDocDbEndpointUri();
            _docDbPrimaryKey = _settingServices.GetDocDbApiKey();
            _docDbDatabaseName = _settingServices.GetDocDbDatabaseName();
            _docDbDigitalMainCollectionName = _settingServices.GetDocDbMainCollectionName();
            _docDbSingletonClient = new CosmosClient(_settingServices.GetDocDbEndpointUri(), settingServices.GetDocDbApiKey());
            _cosmosContainer = _docDbSingletonClient.GetContainer(_docDbDatabaseName, _docDbDigitalMainCollectionName);
        }
        public async Task<BookModel> AddBookService(BookModel newBookDetails)
        {
            try
            {
                if (newBookDetails == null)
                    throw new ArgumentNullException(nameof(newBookDetails));

                newBookDetails.BookID = Guid.NewGuid().ToString();
                using (var response = _cosmosContainer
                    .CreateItemAsync<BookModel>(newBookDetails,
                    new PartitionKey(newBookDetails.BookID)))
                {
                    return response.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBookService(string BookID)
        {
            try
            {
                var book = _cosmosContainer.GetItemLinqQueryable<BookModel>(true)
                    .Where(x => x.BookID == BookID).AsEnumerable().FirstOrDefault();

                if (book == null)
                    throw new ArgumentNullException("Book doesn't exist");

                await _cosmosContainer.DeleteItemAsync<BookModel>(book.BookID, new PartitionKey(book.BookID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksService()
        {
            try
            {
                var response = _cosmosContainer.GetItemLinqQueryable<BookModel>(true);
                return response.ToList<BookModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BookModel> GetBookService(string BookID)
        {
            try
            {
                var response = _cosmosContainer.GetItemLinqQueryable<BookModel>(true)
                    .Where(x => x.BookID == BookID).AsEnumerable().FirstOrDefault();

                if (response == null)
                    throw new ArgumentNullException(nameof(response));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> UpdateBookService(string BookID, BookModel changedBookDetails)
        {
            try
            {
                var book = _cosmosContainer.GetItemLinqQueryable<BookModel>(true)
                    .Where(x => x.BookID == BookID).AsEnumerable().FirstOrDefault();

                if (book == null)
                    throw new ArgumentNullException("Book doesn't exist");

                if (changedBookDetails == null)
                    throw new ArgumentNullException(nameof(changedBookDetails));

                await _cosmosContainer.ReplaceItemAsync<BookModel>(changedBookDetails, book.BookID,
                    new PartitionKey(book.BookID));
                List<BookModel> result = new()
                {
                    book,
                    changedBookDetails
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
