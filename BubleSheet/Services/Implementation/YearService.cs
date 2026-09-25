using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class YearService(ISubject subject,ISubjectService subjectService,IYear year,IBunnyStorageService bunnyStorageService,IUnitOfWork unitOfWork) : IYearService
    {
        private readonly IYear _Year = year;
        private readonly IBunnyStorageService _bunnyStorageService = bunnyStorageService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISubjectService _subjectService = subjectService;
        private readonly ISubject _subject = subject;
        public async Task<YearsResponseDto> AddAsync(AddYearDto addYearDto)
        {
            string? imgLink = null;
            if (addYearDto.Img != null) { 
                var UploadedImg = await _bunnyStorageService.UploadImageAsync(addYearDto.Img, "YearsImgs");
                imgLink = UploadedImg.Url;
            }

                AcademicYear academicYear = new AcademicYear(
                addYearDto.YearName,
                addYearDto.Level,
                imgLink
            );
            await _Year.AddYearAsync(academicYear);
            return new YearsResponseDto
            {
                ImgLink = academicYear.imgLink,
                yearId = academicYear.AcademicYearId,
                YearName = academicYear.Name
            };
        }
        public async Task<YearsResponseDto> EditYear(EditYearDto dto)
        {
            var year = await _Year.GetLevelById(dto.Id);

            string? imgLink = year.imgLink;

            if (dto.img != null)
            {
                if(year.imgLink != null)
                    await _bunnyStorageService.DeleteAsync(year.imgLink);
                var uploadedImg = await _bunnyStorageService.UploadImageAsync(
                    dto.img,
                    "YearsImgs"
                );

                imgLink = uploadedImg.Url;
            }

            year.Update(dto.YearName, imgLink);

            await _unitOfWork.SaveChangesAsync();
            return new YearsResponseDto
            {
                ImgLink = _bunnyStorageService.GenerateSecureUrl(imgLink),
                yearId = year.AcademicYearId,
                YearName = year.Name
            };
        }
        public async Task DeleteYear(int yearId)
        {
            var subjects = await _subject.GetAllSubjectsByYearId(yearId);

            var subjectIds = subjects
                .Select(s => s.SubjectId)
                .ToList();

            if (subjectIds.Any())
            {
                await _subjectService.DeleteSubjects(subjectIds);
            }

            await _Year.DeleteYear(yearId);
        }
        public async Task DeleteImgYear(int yearId)
        {
            var Year = await _Year.GetLevelById(yearId);
            if (Year.imgLink == null || Year == null)
                return;

            await _bunnyStorageService.DeleteAsync(Year.imgLink);
            Year.Update(Year.Name, null);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
