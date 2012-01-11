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
using System.Data.Linq;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;
using Account_Assistant.ViewModels;
using Microsoft.Phone.Shell;

namespace Account_Assistant
{
    public partial class EditDescription : PhoneApplicationPage
    {

        public Charge selectedItemDataChargeDestination2;
        public Loan   selectedItemDataLoanDestination2;



        Func<AppDbContext, String, Loan> GetLoan = CompiledQuery.Compile(
                                                                           ( AppDbContext Appcontext, String person ) =>
                                                                            (from l in Appcontext.Loans
                                                                             where l.person.Equals(person)
                                                                             select l).FirstOrDefault());



        Func<AppDbContext, String, Charge> GetCharge = CompiledQuery.Compile(
                                                                         ( AppDbContext Appcontext, String person ) =>
                                                                          (from c in Appcontext.Charges
                                                                           where c.person.Equals(person)
                                                                           select c).FirstOrDefault());


        bool newInstance = true;
        EditDescriptionViewModel viewModel; 

        public EditDescription()
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
            ApplicationBarIconButton appBarButtonCancel = new ApplicationBarIconButton(new Uri("icons/appbar.close.rest.png", UriKind.Relative));
            appBarButtonAccept.Text = AppResources.Accept;
            appBarButtonCancel.Text = AppResources.Cancel;
            ApplicationBar.Buttons.Add(appBarButtonAccept);
            ApplicationBar.Buttons.Add(appBarButtonCancel);

            appBarButtonAccept.Click +=new EventHandler(appBarButtonAccept_Click);
            appBarButtonCancel.Click += new EventHandler(appBarButtonCancel_Click);
        }

        void appBarButtonCancel_Click( object sender, EventArgs e )
        {
            NavigationService.GoBack();
        }

        void appBarButtonAccept_Click( object sender, EventArgs e )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                    selectedItemDataChargeDestination2 = GetCharge(context, viewModel.Person);
                    selectedItemDataChargeDestination2.description = TextBoxDescription.Text;

                    context.SubmitChanges();

                }
                else
                {
                    selectedItemDataLoanDestination2 = GetLoan(context, viewModel.Person);

                    selectedItemDataLoanDestination2.description = TextBoxDescription.Text;

                    context.SubmitChanges();

                }
            }

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
                        viewModel = (EditDescriptionViewModel)State["viewModel"];
                    }
                    else
                    {
                        if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                        {

                            if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                            {

                                viewModel = new EditDescriptionViewModel(selectedItemDataChargeDestination2.person,
                                                                    selectedItemDataChargeDestination2.description);
                            }
                            else 
                            {
                                viewModel = new EditDescriptionViewModel(selectedItemDataLoanDestination2.person,
                                                                   selectedItemDataLoanDestination2.description);
                            }
                        }
                    }
                }
                this.DataContext = viewModel;
            }

            newInstance = false;
        }
        

       

       

        protected override void OnNavigatedFrom( NavigationEventArgs e )
        {
            base.OnNavigatedFrom(e);

            if(e.NavigationMode != NavigationMode.Back)

            {
                viewModel.Description = TextBoxDescription.Text;
                this.State["viewModel"] = viewModel;
            }

            PersonInfo personInfo = e.Content as PersonInfo;
            if(personInfo != null)
            {

                PersonInfo destination = e.Content as PersonInfo;
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {   using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
                    {
                        if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                        {
                            destination.selectedItemDataChargeDestination = (from c in context.Charges
                                                                           where c.person.Equals(viewModel.Person)
                                                                           select c).FirstOrDefault();

                            destination.DataContext = destination.selectedItemDataChargeDestination;
                        }
                        else
                        {
                            destination.selectedItemDataLoanDestination = (from l in context.Loans
                                                                           where l.person.Equals(viewModel.Person)
                                                                           select l).FirstOrDefault();
 
                            destination.DataContext = destination.selectedItemDataLoanDestination;
                        }
                    }
                }
            }
        }
        

        
    }
}
