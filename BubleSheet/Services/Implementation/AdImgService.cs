using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class AdImgService(IImgAd imgAd, IBunnyStorageService bunnyStorageService,IUnitOfWork unitOfWork) : IImgadService
    {
        private readonly IImgAd _imgAd = imgAd;
        private readonly IBunnyStorageService _bunnyStorageService = bunnyStorageService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<ImgAdDTO>> GetAllImgAds()
        {
            var ads = await _imgAd.GetAllAds();

            return ads.Select(ad => new ImgAdDTO
            {
                ID = ad.Id,
                ImgLink = _bunnyStorageService.GenerateSecureUrl(ad.ImgLink),
                Level = ad.Level
            }).ToList();
        }
        public async Task<ImgAdDTO> GetImgAdByLevel(Levels level)
        {
            var ad = await _imgAd.GetAdByLevel(level);

            return new ImgAdDTO
            {
                ID = ad.Id,
                ImgLink = _bunnyStorageService.GenerateSecureUrl(ad.ImgLink),
                Level = ad.Level
            };
        }
        public async Task<ImgAdDTO> AddAdAsync(AddImgAdDTO addImgAdDTO)
        {
            if (addImgAdDTO.Img == null || addImgAdDTO.Img.Length == 0)
                throw new ArgumentException("Image is required.");

            var oldAd = await _imgAd.GetAdByLevel(addImgAdDTO.Level);

            var uploadResult = await _bunnyStorageService.UploadImageAsync(
                addImgAdDTO.Img,
                "ads"
            );

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (oldAd != null)
                {
                    await _imgAd.DeleteAd(oldAd.Id);
                }

                var img = new ImgAd(
                    uploadResult.Url,
                    addImgAdDTO.Level
                );

                await _imgAd.AddAsync(img);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                if (oldAd != null && !string.IsNullOrEmpty(oldAd.ImgLink))
                {
                    await _bunnyStorageService.DeleteAsync(oldAd.ImgLink);
                }

                return new ImgAdDTO
                {
                    ID = img.Id,
                    ImgLink = _bunnyStorageService.GenerateSecureUrl(img.ImgLink),
                    Level = img.Level
                };
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                await _bunnyStorageService.DeleteAsync(uploadResult.Url);

                throw;
            }
        }
        public async Task DeleteAdAsync(int id)
        {
            var ad = await _imgAd.GetAdById(id);

            if (ad == null)
                throw new KeyNotFoundException("Advertisement not found.");

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _imgAd.DeleteAd(ad.Id);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                if (!string.IsNullOrEmpty(ad.ImgLink))
                {
                    await _bunnyStorageService.DeleteAsync(ad.ImgLink);
                }
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
