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
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Globalization;
using System.Data.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Resources;
using Account_Assistant.ViewModels;
using System.Windows.Navigation;

namespace Account_Assistant
{
    

    public partial class AddNew : PhoneApplicationPage
    {
        Func<AppDbContext, String, Charge> GetNumberReferencesAlreadyExistInChargeRegister = CompiledQuery.Compile(
                                                                                                  ( AppDbContext Appcontext, String personchoosen ) =>
                                                                                                              (from c in Appcontext.Charges
                                                                                                               where c.person.Equals(personchoosen)
                                                                                                               select c).FirstOrDefault());
        Func<AppDbContext, String, Loan> GetNumberReferencesAlreadyExistInLoanRegister = CompiledQuery.Compile(
                                                                                                    ( AppDbContext Appcontext, String personchoosen ) =>
                                                                                                                (from l in Appcontext.Loans
                                                                                                                 where l.person.Equals(personchoosen)
                                                                                                                 select l).FirstOrDefault());
        private byte[] imageaux;
        public Charge auxcharge { get; set; }
        public Loan auxloan { get; set; }


        bool newInstance = true;
        AddModelView viewModel; 

        public AddNew()
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
            ApplicationBarIconButton appBarButtonSave = new ApplicationBarIconButton(new Uri("icons/appbar.save.rest.png", UriKind.Relative));
            ApplicationBarIconButton appBarButtonCamera = new ApplicationBarIconButton(new Uri("icons/appbar.feature.camera.rest.png", UriKind.Relative));
            appBarButtonSave.Text = AppResources.Save;
            appBarButtonCamera.Text = AppResources.Camera;
            ApplicationBar.Buttons.Add(appBarButtonSave);
            ApplicationBar.Buttons.Add(appBarButtonCamera);

            appBarButtonSave.Click +=new EventHandler(appBarButtonSave_Click);
            appBarButtonCamera.Click += new EventHandler(appBarButtonCamera_Click);

        }

        void appBarButtonCamera_Click( object sender, EventArgs e )
        {
            CameraCaptureTask camCapture = new CameraCaptureTask();
            camCapture.Completed += new EventHandler<PhotoResult>(camCapture_Completed);
            camCapture.Show();
        }

        void camCapture_Completed( object sender, PhotoResult e )
        {


            if(e.TaskResult == TaskResult.OK)
            {
                AddNewItem(e.ChosenPhoto);

                BitmapImage img = new BitmapImage();

                img.SetSource(e.ChosenPhoto);
                ImageReference.Source = img;

            }
        }

        public void AddNewItem( Stream image )
        {
            byte[] byteArray = null;
            using(MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap wb = new WriteableBitmap(200, 200);
                wb.LoadJpeg(image);
                wb.SaveJpeg(ms, 200, 200, 0, 60);
                byteArray = ms.ToArray();
            }



            imageaux = byteArray;

        }

        void appBarButtonSave_Click( object sender, EventArgs e )
        {
            if(ChargeRadioButton.IsChecked == true)
            {
                TextBoxPopupConfirm.Text = AppResources.TextBoxPopupConfirm1 + TextBoxAmount.Text + AppResources.TextBoxPopupConfirm2 + TextBlockPersonChoosen.Text + AppResources.TextBoxPopupConfirm3C;
                popupConfirm.IsOpen = true;

            }
            else
            {
                TextBoxPopupConfirm.Text = AppResources.TextBoxPopupConfirm1 + TextBoxAmount.Text + AppResources.TextBoxPopupConfirm2 + TextBlockPersonChoosen.Text + AppResources.TextBoxPopupConfirm3L;
                popupConfirm.IsOpen = true;
            }
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
                        viewModel = (AddModelView)State["viewModel"];

                        if(viewModel.chargeRadioButton == true)
                        {
                            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                            {
                                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(false))
                                    IsolatedStorageSettings.ApplicationSettings["situation"] = true;

                            }
                        }
                        else
                        {
                            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                            {
                                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                                    IsolatedStorageSettings.ApplicationSettings["situation"] = false;

                            }
                        }
                        
                    }
                    else
                    {
                        viewModel = new AddModelView();
                            
                    }
                }
                this.DataContext = viewModel;
            }

            newInstance = false;
        }

        private void UpdateTileCharge( Charge charge )
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

                    IsolatedStorageSettings.ApplicationSettings["TilePerson"] = charge.person;
                    IsolatedStorageSettings.ApplicationSettings["TileSituation"] = "Charge";

                    appTData.BackTitle = AppResources.TileOwesYouMoney;
                    appTile.Update(appTData);
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
                    appTData.BackContent = AppResources.TileYouOweMoneyTo;
                    appTData.BackTitle = loan.person;

                    IsolatedStorageSettings.ApplicationSettings["TilePerson"] = loan.person;
                    IsolatedStorageSettings.ApplicationSettings["TileSituation"] = "Loan";


                    appTile.Update(appTData);
                }
            }
        }

        private void newCharge(AppDbContext context  )
        {



               

                ReferencesCharge referCharge = new ReferencesCharge
                {

                    amount = Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture),2),
                    date = DateTime.Now,
                    reference = TextBoxReference.Text,
                    image = imageaux,
                };

               
                Charge chargeObj= new Charge
                  {
                       
                      amount = Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture),2),
                      
                      phone = TextBlockPersonChoosenPhone.Text.ToString(),
                      person = TextBlockPersonChoosen.Text,
                      NumberReferences = 1,
                      ReferencesCharge =new EntitySet<ReferencesCharge>()
                        {
                            referCharge

                        },

                  };

                referCharge.chargeId = chargeObj.chargeId;

                UpdateTileCharge(chargeObj);

                context.Charges.InsertOnSubmit(chargeObj);
                context.SubmitChanges();

                referCharge.chargeId = chargeObj.chargeId;
                context.SubmitChanges();

                
            
        }

        private void newLoan(AppDbContext context)
        {
           

            ReferencesLoan referLoan = new ReferencesLoan
            {

                amount = Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture),2),
                date = DateTime.Now,
                reference = TextBoxReference.Text,
                image = imageaux,
            };

            Loan loanObj= new Loan
            {

                amount = Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture),2),
                phone = TextBlockPersonChoosenPhone.Text.ToString(),
                person = TextBlockPersonChoosen.Text,
                NumberReferences = 1,
                ReferencesLoan = new EntitySet<ReferencesLoan>()
                        {
                            referLoan,

                        },

            };

            referLoan.loanId = loanObj.loanId;

            UpdateTileLoan(loanObj);

            context.Loans.InsertOnSubmit(loanObj);
            context.SubmitChanges();

            referLoan.loanId = loanObj.loanId;
            context.SubmitChanges();

            
            
        }

        private void UpdateCharge( Charge auxcharge,AppDbContext context )
        {
           

            using(context)
            {

                ReferencesCharge referCharge = new ReferencesCharge
                    {
                        chargeId = auxcharge.chargeId,
                        amount = Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture), 2),
                        date = DateTime.Now,
                        reference = TextBoxReference.Text,
                        image = imageaux,
                    };

                auxcharge.amount = auxcharge.amount + Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture),2);
                auxcharge.NumberReferences = auxcharge.NumberReferences + 1;
                

                auxcharge.ReferencesCharge = new EntitySet<ReferencesCharge>()
                {
                     referCharge

                };
                context.ReferencesCharges.InsertOnSubmit(referCharge);
                context.SubmitChanges();
            }
            
        }

        private void UpdateLoan( Loan auxloan, AppDbContext context )
        {
           

            using(context)
            {

                ReferencesLoan referLoan = new ReferencesLoan
                {
                    loanId = auxloan.loanId,
                    amount = Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture), 2),
                    date = DateTime.Now,
                    reference = TextBoxReference.Text,
                    image = imageaux,
                };

                auxloan.amount = auxloan.amount + Math.Round(double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture),2);
                auxloan.NumberReferences = auxloan.NumberReferences + 1;


                auxloan.ReferencesLoan = new EntitySet<ReferencesLoan>()
                {
                     referLoan,

                };
                context.ReferencesLoans.InsertOnSubmit(referLoan);
                context.SubmitChanges();
            }
            
        }

        

       
		
		

        private void ButonCancelPopup_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            if(popupConfirm.IsOpen)
            {
                popupConfirm.IsOpen = false;
                
            }

          
        }



        private void ButonAcceptPopup_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            

            

            if(popupConfirm.IsOpen)
            {
                popupConfirm.IsOpen = false;
            }
            
            using(AppDbContext context = new AppDbContext("Data Source='isostore:/AccountAssistantDb.sdf'"))
            {

                if(ChargeRadioButton.IsChecked == true)
                {
                    auxcharge = GetNumberReferencesAlreadyExistInChargeRegister(context, TextBlockPersonChoosen.Text);
                    if(auxcharge == null)
                    {
                        if(imageaux == null)
                        {
                            
                            StreamResourceInfo sri = Application.GetResourceStream(new Uri("images/ImageDefaultCharges.jpg", UriKind.Relative));
                            AddNewItem(sri.Stream);


                        }
                        newCharge(context);

                        
                    }
                    else
                    {
                        if(imageaux == null)
                        {

                            StreamResourceInfo sri = Application.GetResourceStream(new Uri("images/ImageDefaultCharges.jpg", UriKind.Relative));
                            AddNewItem(sri.Stream);
                        }
                        UpdateCharge(auxcharge, context);
                    }

                    if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                    {
                        if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(false))
                            IsolatedStorageSettings.ApplicationSettings["situation"] = true;
                    }
                }
                else
                {
                    auxloan = GetNumberReferencesAlreadyExistInLoanRegister(context, TextBlockPersonChoosen.Text);
                    if(auxloan == null)
                    {
                         if(imageaux == null)
                            {
                                
                
                                StreamResourceInfo sri = Application.GetResourceStream(new Uri("images/ImageDefaultLoans.jpg", UriKind.Relative));
                                AddNewItem(sri.Stream);
                            }
                        newLoan(context);
                    }
                    else
                    {
                        if(imageaux == null)
                        {


                            StreamResourceInfo sri = Application.GetResourceStream(new Uri("images/ImageDefaultLoans.jpg", UriKind.Relative));
                            AddNewItem(sri.Stream);
                        }
                        UpdateLoan(auxloan, context);
                    }

                    if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                    {
                        if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                            IsolatedStorageSettings.ApplicationSettings["situation"] = false;
                    }
                }



            }
            
           
            NavigationService.GoBack();
            
        }

        
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {


            if (popupConfirm.IsOpen )
            {
               
                popupConfirm.IsOpen = false;
                e.Cancel = true;
            }
            if(ChargeRadioButton.IsChecked == true)
            {
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {

                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(false))
                        IsolatedStorageSettings.ApplicationSettings["situation"] = true;
                }
            }
            else
            {
                if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
                {
                    if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                        IsolatedStorageSettings.ApplicationSettings["situation"] = false;
                }
            }

            base.OnBackKeyPress(e);
        }

        private void TextBoxAmount_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
          

            if ((TextBoxAmount.Text.Contains(".") && e.PlatformKeyCode == 190) || TextBoxAmount.Text.Length > 4 || (TextBoxAmount.Text.Length.Equals(0) && e.PlatformKeyCode == 190 || (TextBoxAmount.Text.Length.Equals(4) && e.PlatformKeyCode == 190)))
            {
                e.Handled = true;
                
            }
        }

       


        private void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            checking_savebutton();
            

        }

        private void checking_savebutton()
        {
            double sum = 0;
            if (!TextBoxAmount.Text.Length.Equals(0))
                sum = double.Parse(TextBoxAmount.Text, CultureInfo.InvariantCulture);

            if (!TextBlockPersonChoosen.Text.Length.Equals(0) && !TextBoxAmount.Text.Length.Equals(0) && sum > 0 && !TextBoxReference.Text.Length.Equals(0))
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            }
            else
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
            }
        }


        private void btnSelectContact_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           PhoneNumberChooserTask addChoser = new PhoneNumberChooserTask();
           addChoser.Completed += new EventHandler<PhoneNumberResult>(addChoser_Completed);
            
            addChoser.Show();
             
        }

        void addChoser_Completed( object sender, PhoneNumberResult e )
        {
            if(e.TaskResult == TaskResult.OK)
            {
                TextBlockPersonChoosen.Text = e.DisplayName;
                TextBlockPersonChoosenPhone.Text = e.PhoneNumber;
                
                if(!TextBoxAmount.Text.Length.Equals(0) && !TextBoxReference.Text.Length.Equals(0))
                {
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
                }


            }
        }
		
		

		private void ChooseSelection_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
            if(IsolatedStorageSettings.ApplicationSettings.Contains("situation"))
            {
                if(IsolatedStorageSettings.ApplicationSettings["situation"].Equals(true))
                {
                    ChargeRadioButton.IsChecked = true;
                }
                else
                {
                    LoanRadioButton.IsChecked = true;
                }
            }

            checking_savebutton();
		}
        
		

      


        protected override void OnNavigatedFrom( NavigationEventArgs e )
        {
            base.OnNavigatedFrom(e);

            if(e.NavigationMode != NavigationMode.Back)
            {

                

                viewModel.chargeRadioButton = ChargeRadioButton.IsChecked;
               
                viewModel.Amount= TextBoxAmount.Text;
                viewModel.Person=TextBlockPersonChoosen.Text;
                viewModel.phone=TextBlockPersonChoosenPhone.Text;
                viewModel.reference = TextBoxReference.Text;
                this.State["viewModel"] = viewModel;
            }
        }

        
    }
}
