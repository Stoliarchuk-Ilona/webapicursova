using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using firstWebAPI.Extentions;
using firstWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWebAPI.Clients
{
    public class DynamoDbClient : IDynamoDbClient, IDisposable
    {
        public string _tableName;
        private readonly IAmazonDynamoDB _dynamoDb;

        public DynamoDbClient(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
            _tableName = Constants.TableName;
        }
        
        
        public async Task Delete(string artist)
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Artist", new AttributeValue{S = artist} },
                }
            };
            var response = _dynamoDb.DeleteItemAsync(request);
        }
            

        public async Task<ArtistDbRepository> GetDataByName(string artist)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Artist", new AttributeValue{S=artist} }
                }
            };
            var response = await _dynamoDb.GetItemAsync(item);
            if (response.Item == null || !response.IsItemSet)
                return null;
            var result = response.Item.ToClass<ArtistDbRepository>();
            return result;
        }

        public async Task PostDataToDb(ArtistDbRepository data)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"Artist", new AttributeValue{S = data.Artist} },
                    {"Description", new AttributeValue{S = data.Description} }
                }
            };
           
            var response = await _dynamoDb.PutItemAsync(request);
       
        }

        public async Task<List<ArtistDbRepository>> GetAll()
        {
            var result = new List<ArtistDbRepository>();
            var request = new ScanRequest
            {
                TableName = _tableName,
            };
            var response = await _dynamoDb.ScanAsync(request);
            if (response.Items == null || response.Items.Count == 0)
                return null;
            foreach(Dictionary<string, AttributeValue> item in response.Items)
            {
                result.Add(item.ToClass<ArtistDbRepository>());
            }
            return result;
        }

        public void Dispose()
        {
            _dynamoDb.Dispose();
        }

    }
}
