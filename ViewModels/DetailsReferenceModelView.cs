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
    public class DetailsReferenceModelView : INotifyPropertyChanged
    {
        private string Reference;
        [DataMember]
        public String reference
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

        private String TypeRefer;
        [DataMember]
        public string typeRefer
        {
            get
            {
                return TypeRefer;
            }
            set
            {
                TypeRefer = value;
                RaisePropertyChanged("typeRefer");
            }
        }
        
        private DateTime Date;
        [DataMember]
        public DateTime date
        {
            get
            {
                return Date;
            }
            set
            {
                Date = value;
                RaisePropertyChanged("date");
            }
        }

        private byte[] Image;
        [DataMember]
        public byte[] image
        {
            get
            {
                return Image;
            }
            set
            {
                Image = value;
                RaisePropertyChanged("date");
            }
        }

        

        public DetailsReferenceModelView( double amount, String reference, DateTime date, byte[] image, String typeRefer )
        {
           
            this.Amount = amount.ToString();
            this.Reference = reference;
            this.Date = date;
            this.Image = image;
            this.TypeRefer = typeRefer;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged( string propertyName )
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

