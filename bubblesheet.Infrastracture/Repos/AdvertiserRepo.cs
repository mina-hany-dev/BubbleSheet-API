using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class AdvertiserRepo(bubblesheetDbContext context) : IAdvertiser
    {
        private readonly bubblesheetDbContext _context = context;
        public async Task AddAdvertiser(Advertisers advertiser)
        {
            await _context.Advertisers.AddAsync(advertiser);
            await Task.CompletedTask;
        }
        public async Task RemoveAdvertiser(Advertisers advertiser)
        {
            _context.Advertisers.Remove(advertiser);
            await Task.CompletedTask;
        }
        public async Task<List<Advertisers>> GetAllAdvertiser()
        {
            return await _context.Advertisers.ToListAsync();
        }
        public async Task<Advertisers?> GetAdById(int Id)
        {
            return await _context.Advertisers.FirstOrDefaultAsync(a => a.Id == Id);
        }
    }
}
