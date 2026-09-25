using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class PDFDTO
    {
        public int PdfId { get; set; }
        public string PdfName { get; set; }
        public string Description { get; set; }
        public double Space { get; set; }
    }
    public class AddPdfDto
    {
        public IFormFile File { get; set; } = null!;
        public string Name { get; set; }
        public string Description { get; set; }
        public int LessonId { get; set; }
    }
}
