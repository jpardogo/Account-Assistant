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
    public class AddModelView :  INotifyPropertyChanged
    {
        private string amount;
        [DataMember]
        public String Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                RaisePropertyChanged("Amount");
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

        private String Reference;
        [DataMember]
        public string reference
        {
            get
            {
                return Reference;
            }
            set
            {
                Reference = value;
                RaisePropertyChanged("reference");
            }
        }

       


        private bool? ChargeRadioButton;
        [DataMember]
        public bool? chargeRadioButton
        {
            get
            {
                return ChargeRadioButton;
            }
            set
            {
                ChargeRadioButton = value;
                RaisePropertyChanged("chargeradioButton");
            }
        }

       

        public AddModelView()
        {

        }

       

        public event PropertyChangedEventHandler PropertyChanged;
        

        private void RaisePropertyChanged( string propertyName )
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
