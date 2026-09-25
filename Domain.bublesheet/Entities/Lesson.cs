using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Lesson
    {
        [Key]
        public int LessonID { get; private set; }

        public string LessonName { get; private set; }
        public string? Description { get; private set; }
        public int Index { get; private set; }
        public int SubjectId { get; private set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; private set; }
        private readonly List<QuestionBank> _questionBanks = new();
        public IReadOnlyCollection<QuestionBank> QuestionBanks => _questionBanks.AsReadOnly();

        private readonly List<Exam> _exams = new();
        public IReadOnlyCollection<Exam> Exams => _exams.AsReadOnly();

        private readonly List<PdfFile> _pdfFiles = new();
        public IReadOnlyCollection<PdfFile> pdfFiles => _pdfFiles.AsReadOnly();
        private readonly List<StudentLesson> _studentLessons = new();

        public IReadOnlyCollection<StudentLesson> StudentLessons =>
            _studentLessons.AsReadOnly();

        private Lesson() { }

        public Lesson(string lessonName, string? description,int Index, int SubjectId)
        {
            SetLessonName(lessonName);
            Description = description;
            this.Index = Index;
            this.SubjectId = SubjectId;
        }

        public void SetLessonName(string lessonName)
        {
            if (string.IsNullOrWhiteSpace(lessonName))
                throw new ArgumentException("Lesson name is required");

            LessonName = lessonName.Trim();
        }

        public void AddQuestionBank(QuestionBank bank)
        {
            if (bank == null)
                throw new ArgumentNullException(nameof(bank));

            if (_questionBanks.Any(x => x == bank))
                return;

            _questionBanks.Add(bank);
        }

        public void RemoveQuestionBank(QuestionBank bank)
        {
            if (bank == null)
                throw new ArgumentNullException(nameof(bank));

            _questionBanks.Remove(bank);
        }

        public void AddExam(Exam exam)
        {
            if (exam == null)
                throw new ArgumentNullException(nameof(exam));

            if (_exams.Any(x => x == exam))
                return;

            _exams.Add(exam);
        }

        public void RemoveExam(Exam exam)
        {
            if (exam == null)
                throw new ArgumentNullException(nameof(exam));

            _exams.Remove(exam);
        }
        public void AddPdf(PdfFile pdf)
        {
            if (pdf == null)
                throw new ArgumentNullException(nameof(pdf));

            if (_pdfFiles.Any(x => x == pdf))
                return;

            _pdfFiles.Add(pdf);
        }
        public void RemovePdf(PdfFile pdf)
        {
            if (pdf == null)
                throw new ArgumentNullException(nameof(pdf));

            _pdfFiles.Remove(pdf);
        }
        public void Update(string lessonName, string? description)
        {
            SetLessonName(lessonName);
            Description = description;
        }
    }
}
