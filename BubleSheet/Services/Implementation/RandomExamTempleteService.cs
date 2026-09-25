using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class RandomExamTempleteService(IRandomExamTemplate template) :IRandomExamTempleteService
    {
        private readonly IRandomExamTemplate _randomexamTemplet = template;
        public async Task<RandomExamTemplate?> updateAsync(UpdateRandomExamTemplate template)
        {
            var LessonRandom = await _randomexamTemplet.GetByLessonIdAsync(template.LessonId);

            if (LessonRandom == null)
                return null;

            LessonRandom.SetDuration(template.Duration);
            LessonRandom.SetQuestionCount(template.QuestionCount);
            var newrand = await _randomexamTemplet.UpdateAsync(LessonRandom);
            return newrand;
        }
    }
}
