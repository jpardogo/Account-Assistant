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
using System.IO.IsolatedStorage;
using System.Data.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Data;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using Account_Assistant.ViewModels;
using System.Runtime.Serialization;
using System.Globalization;

namespace Account_Assistant
{
    public partial class PersonInfo : PhoneApplicationPage
    {
        public Charge selectedItemDataChargeDestination;
        public Loan selectedItemDataLoanDestination;


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
         InfoPersonViewModel viewModel; 

        public PersonInfo( )
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
            ApplicationBarIconButton appBarButtonDelete = new ApplicationBarIconButton(new Uri("icons/appbar.delete.rest.png", UriKind.Relative));
            ApplicationBarIconButton appBarButtonEdit = new ApplicationBarIconButton(new Uri("icons/appbar.edit.rest.png", UriKind.Relative));
            appBarButtonDelete.Text=AppResources.Delete;
            appBarButtonEdit.Text=AppResources.Edit;
            ApplicationBar.Buttons.Add(appBarButtonDelete);
            ApplicationBar.Buttons.Add(appBarButtonEdit);

            appBarButtonEdit.Click += new EventHandler(appBarButtonEdit_Click);
            appBarButtonDelete.Click += new EventHandler(appBarButtonDelete_Click);
        }

        void appBarButtonDelete_Click( object sender, EventArgs e )
        {
            popupConfirm.IsOpen = true;

            ButtonCall.Visibility = Visibility.Collapsed;

            ButtonText.Visibility = Visibility.Collapsed;
        }

        void appBarButtonEdit_Click( object sender, EventArgs e )
        {
            NavigationService.Navigate(new Uri("/EditDescription.xaml", UriKind.Relative));
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
                        viewModel = (InfoPersonViewModel)State["viewModel"];
                        if(selectedItemDataChargeDestination != null || selectedItemDataLoanDestination != null)
                        {
                            if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                            {
                                viewModel.description = selectedItemDataChargeDestination.description;
                            }
                            else
                            {
                                viewModel.description = selectedItemDataLoanDestination.description;
                            }

                        }
                        
                    }
                    else
                    {
                        if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                        {

                            if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                            {
                                viewModel = new InfoPersonViewModel(selectedItemDataChargeDestination.person,
                                                                    selectedItemDataChargeDestination.amount,
                                                                    selectedItemDataChargeDestination.NumberReferences,
                                                                    selectedItemDataChargeDestination.phone,
                                                                    selectedItemDataChargeDestination.description,
                                                                    selectedItemDataChargeDestination.chargeId);
                            }
                            else
                            {
                                viewModel = new InfoPersonViewModel(selectedItemDataLoanDestination.person,
                                                                    selectedItemDataLoanDestination.amount,
                                                                    selectedItemDataLoanDestination.NumberReferences,
                                                                    selectedItemDataLoanDestination.phone,
                                                                    selectedItemDataLoanDestination.description,
                                                                    selectedItemDataLoanDestination.loanId);
 
                            }
                        }
                    }
                }
                this.DataContext = viewModel;
            }

            newInstance = false;
                   
                    
            



            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {

                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                   

                  
                       TextBlockAmount1.Text = AppResources.TotalCharges;
                       TextBlockReferences1.Text = AppResources.ReferencesCharges;
                       PanoramaItem1.Header = AppResources.Charges;
                       PanoramaItem2.Header = AppResources.Loans;
                  
                    using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
                    {
                        



                        ListBox1.ItemsSource = (from rc in context.ReferencesCharges
                                                where rc.chargeId.Equals(viewModel.id)
                                                orderby rc.date descending
                                                select rc).ToList();

                        Loan auxloan = (from l in context.Loans
                                        where l.person.Equals(viewModel.person)
                                        select l).FirstOrDefault();

                       

                        if(auxloan != null)
                        {

                            ListBox2.ItemsSource = (from rl in context.ReferencesLoans
                                                    where rl.loanId.Equals(auxloan.loanId)
                                                    orderby rl.date descending
                                                    select rl).ToList();
                        }
                    }



                }
                else
                {



                   
                        TextBlockAmount1.Text = AppResources.TotalLoans;
                        TextBlockReferences1.Text = AppResources.ReferencesLoans;
                        PanoramaItem1.Header = AppResources.Loans;
                        PanoramaItem2.Header = AppResources.Charges;
                  
                   

                  
                    

                   

                    using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
                    {
                        
                        ListBox1.ItemsSource = (from rl in context.ReferencesLoans
                                                where rl.loanId.Equals(viewModel.id)
                                                orderby rl.date descending
                                                select rl).ToList();

                       


                        Charge auxcharge = (from c in context.Charges
                                            where c.person.Equals(viewModel.person)
                                            select c).FirstOrDefault();

                        if(auxcharge != null)
                        {
                            ListBox2.ItemsSource = (from rc in context.ReferencesCharges
                                                    where rc.chargeId.Equals(auxcharge.chargeId)
                                                    orderby rc.date descending
                                                    select rc).ToList();
                        }
                    }

                }

            }
        }

        





        private void PanoramaItem( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
        {


            if(Panorama.SelectedIndex.Equals(0))
            {
                if(ListBox1.Items.Count != 0)
                {
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
                    (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
                }


            }
            else
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = false;
            }
        }

        private void ButonAcceptPopup_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {


                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                    Charge chargeToDelete   = GetCharge(context, viewModel.person);
                    context.Charges.DeleteOnSubmit(chargeToDelete);

                   
                    if(ListBox2.Items.Count!=0)
                    {
                        Loan loanToDelete = GetLoan(context, viewModel.person);
                        context.Loans.DeleteOnSubmit(loanToDelete);
                    }

                    UpdateTileCharge(chargeToDelete);
                    
                   


                }
                else
                {


                    Loan loanToDelete = GetLoan(context, viewModel.person);
                    context.Loans.DeleteOnSubmit(loanToDelete);
                    Charge chargeToDelete;
                    if(ListBox2.Items.Count != 0)
                    {
                        chargeToDelete = GetCharge(context, viewModel.person);
                        context.Charges.DeleteOnSubmit(chargeToDelete);

                    }

                    UpdateTileLoan(loanToDelete);//UPDATE TILE
                }
               

                context.SubmitChanges();
            }

            
            popupConfirm.IsOpen = false;

            NavigationService.GoBack();
        }


        private void UpdateTileLoan(Loan loan )
        {
            //Update Tile
            if(IsolatedStorageSettings.ApplicationSettings.Contains("TilePerson"))
            {

                if(IsolatedStorageSettings.ApplicationSettings["TilePerson"].Equals(loan.person))
                {
                    ShellTile appTile = ShellTile.ActiveTiles.First();
                    StandardTileData appTData = new StandardTileData();
                    appTData.BackBackgroundImage = new Uri("images/BackBackgroundRed.png", UriKind.Relative);
                    appTData.BackTitle = loan.person;
                    appTData.BackContent = AppResources.TileYouPaid;
                    appTile.Update(appTData);
                }
            }
        }

        private void UpdateTileCharge( Charge charge )
        {
            //Update Tile
            if(IsolatedStorageSettings.ApplicationSettings.Contains("TilePerson"))
            {

                if(IsolatedStorageSettings.ApplicationSettings["TilePerson"].Equals(charge.person))
                {
                    ShellTile appTile = ShellTile.ActiveTiles.First();
                    StandardTileData appTData = new StandardTileData();
                    
                    appTData.BackTitle = AppResources.TileHasPaid;
                    appTData.BackContent = charge.person ;
                    appTile.Update(appTData);
                }
            }

           
        }


        private void ButonCancelPopup_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            popupConfirm.IsOpen = false;
            ButtonCall.Visibility = Visibility.Visible;
          	ButtonText.Visibility = Visibility.Visible;
             
        }

        protected override void OnBackKeyPress( System.ComponentModel.CancelEventArgs e )
        {


            if(popupConfirm.IsOpen)
            {
                popupConfirm.IsOpen = false;
                e.Cancel = true;
                ButtonCall.Visibility = Visibility.Visible;

                ButtonText.Visibility = Visibility.Visible;
            }
        }

        

        protected override void OnNavigatedFrom( NavigationEventArgs e )
        {
            base.OnNavigatedFrom(e);

            


            if(e.NavigationMode != NavigationMode.Back)
            {
                if(selectedItemDataChargeDestination != null || selectedItemDataLoanDestination != null)
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                    {
                        viewModel.description = selectedItemDataChargeDestination.description;
                    }
                    else
                    {
                        viewModel.description = selectedItemDataLoanDestination.description;
                    }
                   
                }
                this.State["viewModel"] = viewModel;
            }
            


            EditDescription editDescription = e.Content as EditDescription;
            if(editDescription != null)
            {

                EditDescription destination = e.Content as EditDescription;
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {
                    using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
                    {
                        if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                        {



                            destination.selectedItemDataChargeDestination2 = (from c in context.Charges
                                                                              where c.person.Equals(viewModel.person)
                                                                              select c).FirstOrDefault();

                            destination.DataContext = destination.selectedItemDataChargeDestination2;

                        }

                        else
                        {
                            destination.selectedItemDataLoanDestination2 = (from l in context.Loans
                                                                            where l.person.Equals(viewModel.person)
                                                                            select l).FirstOrDefault();
                        }
                    }
                }
            }

            DetailsReference personInfo = e.Content as DetailsReference;
            if(personInfo != null)
            {

                DetailsReference destination = e.Content as DetailsReference;

                    if(selectedReferenceCharge != null)
                    {
                        destination.selectedItemListBoxReferenceCharge = selectedReferenceCharge;
                        destination.DataContext = destination.selectedItemListBoxReferenceCharge;
                        
                    }
                    else
                    {
                        destination.selectedItemListBoxReferenceLoan = selectedReferenceLoan;
                        destination.DataContext = destination.selectedItemListBoxReferenceLoan;
                        
                    }
                    selectedReferenceCharge = null;
                    selectedReferenceLoan = null;
            }
        }

        private void ListBox1DeleteItemContextMenu_Click( object sender, RoutedEventArgs e )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                    {

                        
                        int id = ListBoxDeleteItemContextMenuCharge(sender);


                        ListBox1.ItemsSource = (from rc in context.ReferencesCharges
                                                where rc.chargeId.Equals(id)
                                                orderby rc.date descending
                                                select rc).ToList();


                    }
                    else
                    {
                       

                        int id = ListBoxDeleteItemContextMenuLoan(sender);


                        ListBox1.ItemsSource = (from rl in context.ReferencesLoans
                                                where rl.loanId.Equals(id)
                                                orderby rl.date descending
                                                select rl).ToList();
                    }
                }
            }
        }

        private void ListBox2DeleteItemContextMenu_Click( object sender, RoutedEventArgs e )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(false))
                    {
                        int id = ListBoxDeleteItemContextMenuCharge(sender);


                        ListBox2.ItemsSource = (from rc in context.ReferencesCharges
                                                where rc.chargeId.Equals(id)
                                                orderby rc.date descending
                                                select rc).ToList();

                      


                    }
                    else
                    {
                        int id = ListBoxDeleteItemContextMenuLoan(sender);


                        ListBox2.ItemsSource = (from rl in context.ReferencesLoans
                                                where rl.loanId.Equals(id)
                                                orderby rl.date descending
                                                select rl).ToList();



                       
                    }
                }
            }
        }


        //DELETE REFERENCE OF A CHARGE 
        private int ListBoxDeleteItemContextMenuCharge( object sender)
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                //Charge
                ReferencesCharge selectedListBoxItem = ((sender as MenuItem).DataContext) as ReferencesCharge;
                ReferencesCharge ReferenceChargeToDelete = (from rc in context.ReferencesCharges
                                                            where rc.date.Equals(selectedListBoxItem.date)
                                                            select rc).FirstOrDefault();

                int idcharge = ReferenceChargeToDelete.chargeId;
                double amountToDelete = ReferenceChargeToDelete.amount;

                Charge chargeToUpdate   = (from c in context.Charges
                                           where c.chargeId.Equals(idcharge)
                                           select c).FirstOrDefault();

                chargeToUpdate.NumberReferences = chargeToUpdate.NumberReferences - 1;
                chargeToUpdate.amount =Math.Round( chargeToUpdate.amount - amountToDelete,2);


                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {

                    TextBlockAmount.Text = chargeToUpdate.amount.ToString();
                    TextBlockReferences.Text = chargeToUpdate.NumberReferences.ToString();

                   
                }

                if(chargeToUpdate.amount.Equals(0))
                {
                   
                    context.Charges.DeleteOnSubmit(chargeToUpdate);

                    UpdateTileCharge(chargeToUpdate);

                    

                }


                context.ReferencesCharges.DeleteOnSubmit(ReferenceChargeToDelete);
                context.SubmitChanges();

                return idcharge;
            }
        }


        //DELETE REFERENCE OF A LOAN 
        private int ListBoxDeleteItemContextMenuLoan( object sender )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                //Loan
                ReferencesLoan selectedListBoxItem = ((sender as MenuItem).DataContext) as ReferencesLoan;
                ReferencesLoan ReferenceLoanToDelete = (from rl in context.ReferencesLoans
                                                        where rl.date.Equals(selectedListBoxItem.date)
                                                        select rl).FirstOrDefault();

                int idloan = ReferenceLoanToDelete.loanId;
                double amountToDelete = ReferenceLoanToDelete.amount;

                Loan loanToUpdate   = (from l in context.Loans
                                       where l.loanId.Equals(idloan)
                                       select l).FirstOrDefault();

                loanToUpdate.NumberReferences = loanToUpdate.NumberReferences - 1;
                loanToUpdate.amount = Math.Round(loanToUpdate.amount - amountToDelete, 2);


                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(false))
                {
                    TextBlockAmount.Text = loanToUpdate.amount.ToString();
                    TextBlockReferences.Text = loanToUpdate.NumberReferences.ToString();
                }

                if(loanToUpdate.amount.Equals(0))
                {
                    
                    context.Loans.DeleteOnSubmit(loanToUpdate);


                    UpdateTileLoan(loanToUpdate);
                }


                context.ReferencesLoans.DeleteOnSubmit(ReferenceLoanToDelete);
                context.SubmitChanges();

                return idloan;
            }
        }

       

        private void Call_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            PhoneCallTask  callTask = new PhoneCallTask();
            callTask.PhoneNumber = TextBlockPhone.Text;
            callTask.DisplayName = TextBlockName.Text;
            callTask.DisplayName = TextBlockName.Text;
            callTask.Show();

        }



        ReferencesCharge selectedReferenceCharge;
        ReferencesLoan selectedReferenceLoan;
        private void TapItemListBox1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {
                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                    selectedReferenceCharge = (sender as ListBox).SelectedItem as ReferencesCharge;
                    if(selectedReferenceCharge == null)
                        e.Handled = true;
                    else
                        NavigationService.Navigate(new Uri("/DetailsReference.xaml", UriKind.Relative));
                }
                else
                {
                    selectedReferenceLoan = (sender as ListBox).SelectedItem as ReferencesLoan;
                    if(selectedReferenceLoan == null)
                        e.Handled = true;
                    else
                        NavigationService.Navigate(new Uri("/DetailsReference.xaml", UriKind.Relative));
                }
            }
        }

        
        private void TapItemListBox2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {
                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(false))
                {

                    selectedReferenceCharge = (sender as ListBox).SelectedItem as ReferencesCharge;
                    if(selectedReferenceCharge == null)
                        e.Handled = true;
                    else
                        NavigationService.Navigate(new Uri("/DetailsReference.xaml", UriKind.Relative));
                }
                else
                {
                    selectedReferenceLoan = (sender as ListBox).SelectedItem as ReferencesLoan;
                    if(selectedReferenceLoan == null)
                        e.Handled = true;
                    else
                        NavigationService.Navigate(new Uri("/DetailsReference.xaml", UriKind.Relative));
                }
            }
        }

        private void Text_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	SmsComposeTask sms = new SmsComposeTask();
			sms.To = TextBlockPhone.Text;
			
			sms.Show();
        }

        


       

    }
}