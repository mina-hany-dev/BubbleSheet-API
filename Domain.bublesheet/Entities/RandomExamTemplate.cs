using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class RandomExamTemplate
    {
        public int Id { get; private set; }

        public int LessonId { get; private set; }
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; private set; } = null!;

        public int QuestionCount { get; private set; }

        public TimeSpan Duration { get; private set; }

        private RandomExamTemplate() { }

        public RandomExamTemplate(
            int lessonId,
            int questionCount,
            TimeSpan duration)
        {
            SetQuestionCount(questionCount);
            SetDuration(duration);

            LessonId = lessonId;
        }

        public void SetQuestionCount(int questionCount)
        {
            if (questionCount <= 0)
                throw new Exception("Question count must be greater than zero.");

            QuestionCount = questionCount;
        }

        public void SetDuration(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
                throw new Exception("Duration must be greater than zero.");

            Duration = duration;
        }
    }
}
