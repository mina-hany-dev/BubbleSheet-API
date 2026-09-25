using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class StudentScore
    {
        public int Id { get; private set; }
        public int Score { get; private set; }
        public int AcYearId { get; private set; }
        [ForeignKey(nameof(AcYearId))]
        public AcademicYear AcademicYear { get; private set; }
        public int StudentId { get; private set; }
        public Student Student { get; private set; }
        private StudentScore() { }

        public StudentScore( int acYearId, int studentId)
        {
            Score = 0;
            AcYearId = acYearId;
            StudentId = studentId;
        }
        public void AddScore(int score)
        {
            this.Score = this.Score + score;
        }
    }
}