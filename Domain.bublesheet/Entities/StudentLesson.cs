using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class StudentLesson
    {
        public int Id { get; private set; }

        public int StudentId { get; private set; }

        public LessonStatue LessonStatue { get; private set; }
        public int LessonId { get; private set; }

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; private set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; private set; }

        private StudentLesson()
        {
        }

        public StudentLesson(int studentId, int lessonId)
        {
            StudentId = studentId;
            LessonId = lessonId;
            LessonStatue = LessonStatue.NotStart;
        }

        public void StartLesson()
        {
            LessonStatue = LessonStatue.NotEnd;
        }

        public void CompleteLesson()
        {
            LessonStatue = LessonStatue.End;
        }
    }
}
