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
using System.Windows.Navigation;
using System.IO.IsolatedStorage;
using System.Data.Linq;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Threading;


namespace Account_Assistant
{
    public partial class MainPage : PhoneApplicationPage
    {
        BackgroundWorker backroungWorker;
        Popup myPopup;
        // Constructor
        public MainPage( )
        {
           
            InitializeComponent();
            
            myPopup = new Popup() { IsOpen = true, Child = new SplashScreen() };
            backroungWorker = new BackgroundWorker();
            RunBackgroundWorker();
                    

            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                if(!context.DatabaseExists())
                {
                    context.CreateDatabase();


                }
            }
            if(!IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                IsolatedStorageSettings.ApplicationSettings.Add("situation", true);

            if(!IsolatedStorageSettings.ApplicationSettings.Contains("TilePerson"))
                IsolatedStorageSettings.ApplicationSettings.Add("TilePerson", "");
           
            if(!IsolatedStorageSettings.ApplicationSettings.Contains("TileSituation"))
                IsolatedStorageSettings.ApplicationSettings.Add("TileSituation", "");
            
			
        }

        private void RunBackgroundWorker( )
        {
            backroungWorker.DoWork += (( s, args ) =>
            {
                Thread.Sleep(3000);
            });

            backroungWorker.RunWorkerCompleted += (( s, args ) =>
            {
                this.Dispatcher.BeginInvoke(( ) =>
                {
                    this.myPopup.IsOpen = false;
                }
            );
            });
            backroungWorker.RunWorkerAsync();

            BuildApplicationBar();
        }


        private void BuildApplicationBar( )
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButtonAdd = new ApplicationBarIconButton(new Uri("icons/appbar.add.rest.png", UriKind.Relative));
           
            appBarButtonAdd.Text = AppResources.Addnew;
           
            ApplicationBar.Buttons.Add(appBarButtonAdd);


            appBarButtonAdd.Click += new EventHandler(appBarButtonAdd_Click);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.ResetBackTile);
            ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem.Click += new EventHandler(appBarMenuItem_Click);
            
        }

        void appBarMenuItem_Click( object sender, EventArgs e )
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();
            StandardTileData appTData = new StandardTileData();
            appTData.BackTitle = "";
            appTData.BackContent = "";
            appTData.BackBackgroundImage = new Uri("", UriKind.Relative);
            appTile.Update(appTData);
        }

        void appBarButtonAdd_Click( object sender, EventArgs e )
        {
            NavigationService.Navigate(new Uri("/add.xaml", UriKind.Relative));
        }
        
       

       


        List<Charge> myCharges { get; set; }
        private void ButtonCharges_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                myCharges = context.Charges.ToList();

            }

            ListBoxChLo.ItemsSource = myCharges;
            IsolatedStorageSettings.ApplicationSettings["situation"] = true;
        }

        List<Loan> myLoans { get; set; }
        private void ButtonLoans_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                myLoans = context.Loans.ToList();
            }
            ListBoxChLo.ItemsSource = myLoans;
            IsolatedStorageSettings.ApplicationSettings["situation"] = false;
        }

        private void ChooseSelection_Loaded( object sender, System.Windows.RoutedEventArgs e )
        {
            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {
                using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                    {
                        myCharges = context.Charges.ToList();
                        ListBoxChLo.ItemsSource = myCharges;
                        VisualStateManager.GoToState(this, "ChargesPressed", true);
                    }
                    else
                    {
                        myLoans = context.Loans.ToList();
                        ListBoxChLo.ItemsSource = myLoans;
                        VisualStateManager.GoToState(this, "LoansPressed", true);
                    }

                }
            }
        }


        private Charge selectedItemDataChargeMain;
        private Loan selectedItemDataLoanMain;
        private void TapItemListBox( object sender, System.Windows.Input.GestureEventArgs e )
        {
            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {
                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                    selectedItemDataChargeMain = (sender as ListBox).SelectedItem as Charge;
                    if(selectedItemDataChargeMain == null)
                        e.Handled = true;
                    else
                        NavigationService.Navigate(new Uri("/PersonInfo.xaml", UriKind.Relative));
                }
                else
                {
                    selectedItemDataLoanMain = (sender as ListBox).SelectedItem as Loan;
                    if(selectedItemDataLoanMain == null)
                        e.Handled = true;
                    else
                        NavigationService.Navigate(new Uri("/PersonInfo.xaml", UriKind.Relative));
                }

               
               

            }
           

        }

        protected override void OnNavigatedFrom( NavigationEventArgs e )
        {
            base.OnNavigatedFrom(e);
            PersonInfo personInfo = e.Content as PersonInfo;
            if(personInfo != null)
            {
            
                PersonInfo destination = e.Content as PersonInfo;
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                    {
                        destination.selectedItemDataChargeDestination = selectedItemDataChargeMain;
                        destination.DataContext = destination.selectedItemDataChargeDestination;
                    }
                    else
                    {
                        destination.selectedItemDataLoanDestination = selectedItemDataLoanMain;
                        destination.DataContext = destination.selectedItemDataLoanDestination;
                    }
                }
            }
        }


        private void UpdateTileLoan( Loan loan )
        {
            //Update Tile
            if(IsolatedStorageSettings.ApplicationSettings.Contains("TilePerson") && IsolatedStorageSettings.ApplicationSettings.Contains("TileSituation"))
            {

                if(IsolatedStorageSettings.ApplicationSettings["TilePerson"].Equals(loan.person) && IsolatedStorageSettings.ApplicationSettings["TileSituation"].Equals("Loan"))
                {
                    ShellTile appTile = ShellTile.ActiveTiles.First();
                    StandardTileData appTData = new StandardTileData();
                    appTData.BackBackgroundImage = new Uri("images/BackBackgroundRed.png", UriKind.Relative);
                    appTData.BackContent = AppResources.TileYouPaid;
                    appTData.BackTitle = loan.person;
                    
                    appTile.Update(appTData);
                }
            }
        }

        private void UpdateTitleCharge( Charge charge )
        {
            //Update Tile
            if(IsolatedStorageSettings.ApplicationSettings.Contains("TilePerson") && IsolatedStorageSettings.ApplicationSettings.Contains("TileSituation"))
            {

                if(IsolatedStorageSettings.ApplicationSettings["TilePerson"].Equals(charge.person) && IsolatedStorageSettings.ApplicationSettings["TileSituation"].Equals("Charge"))
                {
                    ShellTile appTile = ShellTile.ActiveTiles.First();
                    StandardTileData appTData = new StandardTileData();
                    appTData.BackBackgroundImage = new Uri("images/BackBackgroundGreen.png", UriKind.Relative);
                    appTData.BackContent = charge.person;
                    appTData.BackTitle = AppResources.TileHasPaid;
                    
                    appTile.Update(appTData);
                }
            }
        }

        private void MenuItemDelete_Click( object sender, RoutedEventArgs e )
        {
           

            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                    {
                
                        Charge selectedListBoxItem = ((sender as MenuItem).DataContext) as Charge;
                        Charge chargeToDelete = (from c in context.Charges
                                                  where c.person.Equals(selectedListBoxItem.person)
                                                  select c).FirstOrDefault();
                            
                            


                        UpdateTitleCharge(selectedListBoxItem);
                        context.Charges.DeleteOnSubmit(chargeToDelete);
                        context.SubmitChanges();
                        myCharges = context.Charges.ToList();
                        ListBoxChLo.ItemsSource = myCharges;
                    }
                    else
                    {
                        Loan selectedListBoxItem = ((sender as MenuItem).DataContext) as Loan;
                        Loan loanToDelete = (from l in context.Loans
                                             where l.person.Equals(selectedListBoxItem.person)
                                             select l).FirstOrDefault();
                        UpdateTileLoan(selectedListBoxItem);
                        context.Loans.DeleteOnSubmit(loanToDelete);
                        context.SubmitChanges();
                        myLoans = context.Loans.ToList();
                        ListBoxChLo.ItemsSource = myLoans;
                    }
                }
            }
        }

        private void SetBackBackgroundMainTile_Click( object sender, RoutedEventArgs e )
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();
            StandardTileData appTData = new StandardTileData();
            
            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {

                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                    Charge selectedListBoxItem = ((sender as MenuItem).DataContext) as Charge;
                  
                    appTData.BackTitle = AppResources.TileOwesYouMoney;
                    appTData.BackContent = selectedListBoxItem.person;
                    appTData.BackBackgroundImage = new Uri("images/BackBackgroundGreen.png", UriKind.Relative);

                    IsolatedStorageSettings.ApplicationSettings["TilePerson"] = selectedListBoxItem.person;
                    IsolatedStorageSettings.ApplicationSettings["TileSituation"] = "Charge";

                    
                    appTile.Update(appTData);
                }
                else
                {
                    Loan selectedListBoxItem = ((sender as MenuItem).DataContext) as Loan;
                   
                    appTData.BackTitle = selectedListBoxItem.person;
                    appTData.BackContent = AppResources.TileYouOweMoneyTo;
                    appTData.BackBackgroundImage = new Uri("images/BackBackgroundRed.png", UriKind.Relative);
                    
                    IsolatedStorageSettings.ApplicationSettings["TilePerson"]= selectedListBoxItem.person;
                    IsolatedStorageSettings.ApplicationSettings["TileSituation"]= "Loan";
                    

                    appTile.Update(appTData); 
                }
        
            }
        }

        
       

       

        private void ButtonRateAndReview_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	MarketplaceReviewTask marketOpinion = new MarketplaceReviewTask();
			marketOpinion.Show();
        }

        

        private void PivotItemSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	  if(Pivot.SelectedIndex == 0 )
            {
                while(this.NavigationService.BackStack.Any())
                {
                    this.NavigationService.RemoveBackEntry();
                }
                ApplicationBar.IsVisible = true;

            }
            else
                ApplicationBar.IsVisible = false;
        }

        

    


       
    }
}
