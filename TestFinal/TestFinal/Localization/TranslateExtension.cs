using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mime;
using System.Reflection;
using System.Resources;
using System.Text;
using TestFinal.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestFinal.Localization
{
    // You exclude the 'Extension' suffix when using in XAML
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        const string ResourceId = "UsingResxLocalization.Resx.AppResources";

        private static ResourceManager _resMgr;
        private static int Lang { get; set; }

        public string Text { get; set; }

        public TranslateExtension()
        {
            
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        private static System.Resources.ResourceManager ResManager
        {
            get
            {
                if (!ReferenceEquals(_resMgr, null) && Lang == App.Settings.Language) return _resMgr;

                var srcPath = "TestFinal.Localization.";
                switch (App.Settings.Language)
                {
                    case (int)LanguageKey.English:
                        srcPath += "Translate";
                        break;
                    case (int)LanguageKey.Vietnamese:
                        srcPath += "TranslateBurmeseZawgyi";
                        break;
                    
                }

                Lang = App.Settings.Language;

                var temp =
                    new System.Resources.ResourceManager(
                        srcPath,
                        typeof(English).GetTypeInfo().Assembly);
                _resMgr = temp;
                return _resMgr;
            }
        }

        #region IMarkupExtension implementation

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var translation = _resMgr.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }

        public static string Get(string text, bool allCaps = false, bool colon = false)
        {
            var translation = ResManager.GetString(text ?? "", CultureInfo.CurrentCulture) ?? (text ?? "");
            
            return translation;
        }

        #endregion
    }
}
