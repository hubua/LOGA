using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGAWebApp.Models
{
    public class UserSettings
    {
        public Writing WritingCapitalization { get; set; }
        public bool SaveLearnProgress { get; set; }
        public int SavedLearnProgressLId { get; set; }
    }

    public enum Writing
    {
        Mixed = 0,
        OnlyNuskhuri = 1,
        OnlyAsomtavruli = 2,
    }
}