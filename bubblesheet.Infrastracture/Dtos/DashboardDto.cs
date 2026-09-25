using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class AdminDataResponseDto
    {
        public int CountOfStudent {  get; set; }
        public List<YearsResponseDto> Years { get; set; } = new();
        public List<ImgAdDTO> imgAds { get; set; }
    }
    public class DataResponseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public string School { get; set; }
        public string Gender { get; set; }
        public string ParentphoneNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime? LastLogin { get; set; }
    }
    public class SubjectsDto
    {
        public int SubjectId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Levels Level { get; set; }
        public int LessonCount { get; set; }
        public decimal Price { get; set; }

    }
    public class DataofSubjectDto : SubjectsDto
    {
        public int CountOfLessonsCompelete {  get; set; }
        public int CountOfLessonsNotCompelete { get; set; }
        public int percentage {  get; set; }
    }
    public class SubjectDetailsDto
    {
        public int SubjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Levels Level { get; set; }

        public int LessonCount { get; set; }

        public int CompletedLessons { get; set; }

        public int NotCompletedLessons { get; set; }

        public int Percentage { get; set; }

        public List<LessonDto> Lessons { get; set; } = new();
    }
}
