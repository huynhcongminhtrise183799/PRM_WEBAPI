using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure.Repository
{
	public class ConversationRepository : IConversationRepository
	{
		private readonly IChatDbContext _context;

		public ConversationRepository(IChatDbContext database)
		{
			_context = database;
		}

		public async Task<bool> Add(Conversation conversation)
		{
			try
			{
				await _context.Conversations.InsertOneAsync(conversation);
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}


}
