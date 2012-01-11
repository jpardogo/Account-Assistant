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
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Account_Assistant.ViewModels
{
    [DataContract]
    public class EditDescriptionViewModel : INotifyPropertyChanged
    {
        private string description;
        [DataMember]
        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }

         private string person;
        [DataMember]
        public String Person
        {
            get
            {
                return person;
            }
            set
            {
                person = value;
                RaisePropertyChanged("Person");
            }
        }

        public EditDescriptionViewModel( String person, String description )
        {
            this.person = person;

            this.description = description;
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged( string propertyName )
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
