using Domain.bublesheet.Entities.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class AddImgAdDTO
    {
        public string? ImgLink { get; set; }
        public IFormFile Img { get; set; }
        public Levels Level {  get; set; }
    }
    public class ImgAdDTO : AddImgAdDTO
    {
        public int ID { get; set; }
    }
}
