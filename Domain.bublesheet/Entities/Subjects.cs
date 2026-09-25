using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; private set; }

        public string Name { get; private set; }
        public int YearId { get; private set; }
        [ForeignKey(nameof(YearId))]
        public AcademicYear academicYear { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        private readonly List<Lesson> _Lessons = new();
        public IReadOnlyCollection<Lesson> Lessons => _Lessons.AsReadOnly();
        private Subject() { }

        public Subject(string name, decimal price,int YearId, string? description)
        {
            SetName(name);
            Price = price;
            this.YearId = YearId;
            Description = description;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Subject name is required");

            Name = name;
        }

        public void AddLesson(Lesson Lesson)
        {
            if (Lesson == null)
                throw new ArgumentNullException(nameof(Lesson));

            _Lessons.Add(Lesson);
        }

        public void RemoveLesson(Lesson Lesson)
        {
            if (Lesson == null)
                throw new ArgumentNullException(nameof(Lesson));

            _Lessons.Remove(Lesson);
        }
        public void ChangePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Price cannot be negative");

            Price = price;
        }
        public void Update(string name, decimal price, string? description)
        {
            SetName(name);

            ChangePrice(price);

            Description = description;
        }
    }
}
