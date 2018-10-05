using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGA.WebUI.Models
{
    public class UserSettings
    {
        public bool LearnAsomtavruli { get; set; }
        public bool SaveLearnProgress { get; set; }
        public int SavedLearnProgressLId { get; set; }
    }
}