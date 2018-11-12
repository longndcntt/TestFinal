using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TestFinal.Resources;
using Xamarin.Forms;

namespace TestFinal.Coverter
{
    public class CovertToCurrency : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                CultureInfo TempCulture = new CultureInfo("en-US");
                if (CrossMultilingual.Current.DeviceCultureInfo.ToString().Equals(TempCulture.ToString()))
                {
                    value = double.Parse(value.ToString());
                    value = (double)value/22000;
                    value = string.Format("{0:C2}", value);
                    string s = value.ToString();

                    return s;
                }
                else
                {
                    value = string.Format("{0:C}", value);
                    string s = value.ToString();
                    return s;
                }
                   
                
            }
            return null;
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
