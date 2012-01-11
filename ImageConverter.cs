using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace Account_Assistant
{
    public class ImageConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if(value is byte[])
            {
                BitmapImage image = new BitmapImage();
                WriteableBitmap writeBitmap = new WriteableBitmap(200, 200);
                MemoryStream memoryStream = new MemoryStream(value as byte[]);
                writeBitmap.LoadJpeg(memoryStream);

                image.SetSource(memoryStream);
                return image;
            }
            else
                return null;

            //BitmapImage image = new BitmapImage();
            //string s = System.Convert.ToString(value);
            //image.UriSource = new Uri(s, UriKind.Relative);
            //return value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
