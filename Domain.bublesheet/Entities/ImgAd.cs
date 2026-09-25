using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class ImgAd
    {
        public int Id { get; private set; }
        public string ImgLink { get; private set; }
        public Levels Level { get; private set; }
        private ImgAd()
        {
            
        }
        public ImgAd(string imgLink , Levels level)
        {
            ImgLink = imgLink;
            this.Level = level;
        }
    }
}
