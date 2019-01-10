using System;
using System.Collections.Generic;
using System.Text;
using TestFinal.Enums;

namespace TestFinal.Model
{
    public class AppSettings
    {
        public int Language { get; set; } = (int)LanguageKey.English;
    }
}
