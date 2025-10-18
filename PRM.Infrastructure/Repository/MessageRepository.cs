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
	public class MessageRepository : IMessageRepository
	{
		private readonly IChatDbContext _context;
		private const int  PAGE_SIZE = 5;

		public MessageRepository(IChatDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Add(Messages messages)
		{
			try
			{
				await _context.Messages.InsertOneAsync(messages);
				return true;
			}
			catch (Exception)
			{

				throw;
			}
			 
		}

		public async Task<List<Messages>> GetMessagesByConversationIdAsync(Guid conversationId, int page)
		{
			return await _context.Messages.Find(m => m.ConservationId == conversationId)
				.SortByDescending(m => m.SendAt)
				.Skip((page - 1) * PAGE_SIZE)
				.Limit(PAGE_SIZE)
				.ToListAsync();
		}
	}
}
