using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class AcademicYear
    {
        [Key]
        public int AcademicYearId { get; private set; }
        public string Name { get; private set; }
        public Levels ACLevel { get; private set; }
        public string? imgLink { get; private set; }
        private readonly List<Subject> _subjects = new();
        public IReadOnlyCollection<Subject> Subjects => _subjects.AsReadOnly();
        private AcademicYear() { }
        public AcademicYear(string name, Levels level, string? imgLink)
        {
            SetName(name);
            ACLevel = level;
            this.imgLink = imgLink;
        }
        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Academic year is required");
            
            Name = name;
        }
        public void Update(string name, string? imgLink)
        {
            SetName(name);

            this.imgLink = imgLink;
        }
    }
}