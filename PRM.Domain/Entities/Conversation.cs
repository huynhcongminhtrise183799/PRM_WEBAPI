using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{

	public class Conversation
	{
		[BsonId]
		public Guid ConservationId { get; set; }

		public Guid UserId { get; set; }

		public Guid AdminId { get; set; }

	}
}
