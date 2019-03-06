using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeboenaWebApp.Models
{
    public class WordToTranslate
    {
        public string Word { get; set; }
        public bool? IsTranslatedCorrectly { get; set; }
    }
}