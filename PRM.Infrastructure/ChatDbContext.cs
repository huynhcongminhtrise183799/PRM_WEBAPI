using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure
{
	public interface IChatDbContext
	{
		IMongoCollection<Messages> Messages { get; }
		IMongoCollection<Conversation> Conversations { get; }
	}
	public class ChatDbContext : IChatDbContext
	{
		private readonly IMongoDatabase _database;

		public ChatDbContext(string connectionString, string dbName)
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

			var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
			var client = new MongoClient(clientSettings);

			_database = client.GetDatabase(dbName);
		}


		public IMongoCollection<Messages> Messages
			=> _database.GetCollection<Messages>("Messages");

		public IMongoCollection<Conversation> Conversations
			=> _database.GetCollection<Conversation>("Conversations");
	}
}
