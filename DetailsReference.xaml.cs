using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Account_Assistant.ViewModels;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace Account_Assistant
{
    public partial class DetailsReference : PhoneApplicationPage
    {
        public ReferencesCharge selectedItemListBoxReferenceCharge;
        public ReferencesLoan    selectedItemListBoxReferenceLoan;


        bool newInstance = true;
        DetailsReferenceModelView viewModel;

        public DetailsReference()
        {
            InitializeComponent();
            BuildApplicationBar();
             newInstance = true;
        }

        private void BuildApplicationBar( )
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButtonAccept = new ApplicationBarIconButton(new Uri("icons/appbar.check.rest.png", UriKind.Relative));

            appBarButtonAccept.Text = AppResources.Accept;

            ApplicationBar.Buttons.Add(appBarButtonAccept);


            appBarButtonAccept.Click += new EventHandler(appBarButtonAccept_Click);

           

        }

        void appBarButtonAccept_Click( object sender, EventArgs e )
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e )
        {
            base.OnNavigatedTo(e);

            if(newInstance)
            {
                if(viewModel == null)
                {
                    if(State.Count > 0)
                    {
                        viewModel = (DetailsReferenceModelView)State["viewModel"];
                    }
                    else
                    {
                        if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                        {

                            if(selectedItemListBoxReferenceCharge!=null)
                            {

                                viewModel = new DetailsReferenceModelView(selectedItemListBoxReferenceCharge.amount,
                                                                    selectedItemListBoxReferenceCharge.reference,
                                                                    selectedItemListBoxReferenceCharge.date,
                                                                     selectedItemListBoxReferenceCharge.image,
                                                                    AppResources.Charge);
                            }
                            else
                            {
                                viewModel = new DetailsReferenceModelView(selectedItemListBoxReferenceLoan.amount,
                                                                        selectedItemListBoxReferenceLoan.reference,
                                                                   selectedItemListBoxReferenceLoan.date,
                                                                    selectedItemListBoxReferenceLoan.image,
                                                                   AppResources.Loan);
                            }
                        }
                    }
                }
                this.DataContext = viewModel;
            }

            newInstance = false;
        }

        
       protected override void  OnNavigatedFrom(NavigationEventArgs e)
        {
 	         base.OnNavigatedFrom(e);
             if(e.NavigationMode != NavigationMode.Back)
             {


                 this.State["viewModel"] = viewModel;
             }
        }
       
 
}


   

      

        
}
