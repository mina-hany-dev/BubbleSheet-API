using Domain.bublesheet.Entities.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class YearsResponseDto
    {
        public int yearId { get; set; }
        public string YearName { get; set; }
        public string? ImgLink { get; set; }
    }
    public class AddYearDto
    {
        public string YearName { get; set; }
        public Levels Level { get; set; }
        public IFormFile? Img { get; set; }
    }
    public class EditYearDto
    {
        public int Id { get; set; }
        public string YearName { get; set; }
        public IFormFile? img { get; set; }
    }
}
