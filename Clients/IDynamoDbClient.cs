using firstWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWebAPI.Clients
{
    public interface IDynamoDbClient
    {
        public Task<ArtistDbRepository> GetDataByName(string artist);
        public Task PostDataToDb(ArtistDbRepository data);
        public Task Delete(string artist);
        public Task<List<ArtistDbRepository>> GetAll();
    }
}
