using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class PdfFile
    {
        [Key]
        public int PdfId { get; private set; }
        public string PdfName {  get; private set; }
        public string Description { get; private set; }
        public string PdfLink { get; private set; }
        public double Space {  get; private set; }
        public int LessonID { get; private set; }
        [ForeignKey(nameof(LessonID))]
        public Lesson Lesson { get; private set; }
        private PdfFile() { }
        public PdfFile(string pdfLink, string pdfName , string description, double space , int LessonID)
        {
            SetPdfLink(pdfLink);
            PdfName = pdfName;
            Description = description;
            Space = space;
            this.LessonID = LessonID;
        }
        private void SetPdfLink(string pdfLink)
        {
            if (string.IsNullOrWhiteSpace(pdfLink))
                throw new ArgumentException("Pdf is required");

            PdfLink = pdfLink.Trim();
        }
    }
}