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
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Account_Assistant
{
    public class AppDbContext : DataContext
    {
        
            public AppDbContext( string connectionString ): base(connectionString)
            {

            }
            
            public Table<Charge> Charges
            {
                get
                {
                    return this.GetTable<Charge>();
                }
            }

            public Table<Loan> Loans
            {
                get
                {
                    return this.GetTable<Loan>();
                }
            }

            public Table<ReferencesCharge> ReferencesCharges
            {
                get
                {
                    return this.GetTable<ReferencesCharge>();
                }
            }

            public Table<ReferencesLoan> ReferencesLoans
            {
                get
                {
                    return this.GetTable<ReferencesLoan>();
                }
            }
    }
}
