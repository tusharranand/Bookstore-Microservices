using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Shared.Services
{
    public interface ISettingServices
    {
        string GetDocDbEndpointUri();
        string GetDocDbApiKey();
        string GetDocDbConnectionString();
        string GetDocDbDatabaseName();
        string GetDocDbMainCollectionName();
        int? GetDocDbThroughput();
    }
}
