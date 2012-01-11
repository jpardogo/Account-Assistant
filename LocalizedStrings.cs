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

namespace Account_Assistant
{
    public class LocalizedStrings
    {
        public LocalizedStrings( )
        {
        }

        private static Account_Assistant.AppResources localizedResources = new Account_Assistant.AppResources();

        public Account_Assistant.AppResources LocalizedResources
        {
            get { return localizedResources; }
        }
    }
}
