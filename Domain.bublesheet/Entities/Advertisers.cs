using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Entities
{
    public class Advertisers
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ImgLink { get; private set; }
        private Advertisers() { }
        public Advertisers (string Name ,  string ImgLink)
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ImgLink))
            {
                throw new ArgumentException("All Entities do not fill");
            }
            this.Name = Name;
            this.ImgLink = ImgLink;
        }
    }
}
