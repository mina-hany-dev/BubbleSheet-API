using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class StudentSubjects
    {
        [Key]
        public int Id { get; private set; }
        public int StudentId { get;private set; }
        public int SubjectId { get; private set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; private set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get;  private set; }
        public DateTime EnrolledAt { get; private set; }
        private StudentSubjects() { }
        public StudentSubjects(int studentId, int subjectId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Invalid Student Id");

            if (subjectId <= 0)
                throw new ArgumentException("Invalid Subject Id");

            StudentId = studentId;
            SubjectId = subjectId;
            EnrolledAt = DateTime.UtcNow;
        }
    }
}
