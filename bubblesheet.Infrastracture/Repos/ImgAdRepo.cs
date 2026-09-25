using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class ImgAdRepo : IImgAd
    {
        private readonly bubblesheetDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public async Task UpdateAsync(ImgAd imgAd)
        {
            _context.ImgAds.Update(imgAd);
            await _unitOfWork.SaveChangesAsync();
        }
        public ImgAdRepo(bubblesheetDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ImgAd>> GetAllAds()
        {
            return await _context.ImgAds.ToListAsync();
        }

        public async Task<ImgAd?> GetAdByLevel(Levels level)
        {
            return await _context.ImgAds
                .FirstOrDefaultAsync(x => x.Level == level);
        }

        public async Task AddAsync(ImgAd imgAd)
        {
            await _context.ImgAds.AddAsync(imgAd);
            await Task.CompletedTask;
        }
        public async Task<ImgAd> GetAdById(int Id)
        {
            return await _context.ImgAds.FirstOrDefaultAsync(ia => ia.Id == Id);
        }

        public async Task DeleteAd(int id)
        {
            var ad = await _context.ImgAds.FindAsync(id);

            if (ad == null)
                return;

            _context.ImgAds.Remove(ad);
        }
    }
}
