using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Domain.Entities
{
	public class Messages
	{
		[BsonId]
		public Guid MessageId { get; set; }

		public Guid ConservationId { get; set; }

		public Guid SenderId { get; set; }

		public string Content { get; set; }

		public DateTime SendAt { get; set; }
	}
}
