using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class AdvertisersService(IBunnyStorageService bunnyStorageService,IAdvertiser advertiser , IUnitOfWork unitOfWork) : IAdvertiserService
    {
        private readonly IAdvertiser _advertiser = advertiser;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBunnyStorageService _bunnyStorageService = bunnyStorageService;
        public async Task<List<AdvertisersDto>> GetAllAdvertiser()
        {
            var advertisers = await _advertiser.GetAllAdvertiser();
            
            return advertisers.Select(a => new AdvertisersDto
            {
                Id = a.Id,
                Name = a.Name,
                ImgLink = _bunnyStorageService.GenerateSecureUrl(a.ImgLink)
            }).ToList();
        }
        public async Task AddAsyncAdvertiser(AddAdvertiserDto advertisersDto)
        {
            string Link = string.Empty;
            if(advertisersDto.Img != null)
            {
                var Uploaded = await _bunnyStorageService.UploadImageAsync(advertisersDto.Img,"Ads");
                Link = Uploaded.Url;
            }
            Advertisers ad = new Advertisers(advertisersDto.Name, Link);
            await _advertiser.AddAdvertiser(ad);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAdvertiser(int id)
        {
            var ad = await _advertiser.GetAdById(id);
            if (ad == null)
                throw new ArgumentException("ad Not Found");
            await _bunnyStorageService.DeleteAsync(ad.ImgLink);
            await _advertiser.RemoveAdvertiser(ad);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
