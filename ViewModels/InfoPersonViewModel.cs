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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Account_Assistant.ViewModels
{
    [DataContract]
    public class InfoPersonViewModel : INotifyPropertyChanged
    {
        private string Id;
        [DataMember]
        public String id
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
                RaisePropertyChanged("id");
            }
        }

        private string Person;
        [DataMember]
        public String person
        {
            get
            {
                return Person;
            }
            set
            {
                Person = value;
                RaisePropertyChanged("person");
            }
        }

        private string Amount;
        
        [DataMember]
        public string amount
        {
            get
            {
                return Amount;
            }
            set
            {
                Amount = value;
                RaisePropertyChanged("amount");
            }
        }
        
        private String numberReferences;
        [DataMember]
        public string NumberReferences
        {
            get
            {
                return numberReferences;
            }
            set
            {
                numberReferences = value;
                RaisePropertyChanged("NumberReferences");
            }
        }

        private String Description;
        [DataMember]
        public string description
        {
            get
            {
                return Description;
            }
            set
            {
                Description = value;
                RaisePropertyChanged("description");
            }
        }

        private String Phone;
        [DataMember]
        public string phone
        {
            get
            {
                return Phone;
            }
            set
            {
                Phone = value;
                RaisePropertyChanged("phone");
            }
        }

        public InfoPersonViewModel( String person, double amount, int NumberReferences, String phone, String description, int Id )
        {
            this.Person = person;
            this.Amount = amount.ToString();
            this.numberReferences = NumberReferences.ToString();
            this.Phone = phone;
            this.Description = description;
            this.id = Id.ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged( string propertyName )
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
       

}

