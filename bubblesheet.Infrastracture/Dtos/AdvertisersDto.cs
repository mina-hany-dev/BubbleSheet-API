using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Dtos
{
    public class AdvertisersDto
    {
        public int Id { get; set; }
        public string Name { get;  set; }
        public string ImgLink { get;  set; }
    }
    public class AddAdvertiserDto
    {
        public string Name { get; set; }
        public IFormFile Img { get; set; }
    }
}
