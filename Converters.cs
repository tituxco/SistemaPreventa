    using System;
    using System.Globalization;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Controls.Xaml;

    namespace SistemaPreventa
    {
        public class DateTimeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is DateTime dateTime)
                {
                    return dateTime.ToString("dd/MM/yyyy");
                }

                return string.Empty;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
