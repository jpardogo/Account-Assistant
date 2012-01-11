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

namespace Account_Assistant
{
    [Table(Name = "ReferencesLoan")]
    public class ReferencesLoan
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int referLoanId { get; set; }

        [Column(CanBeNull = false)]
        public int loanId { get; set; }

        [Column(CanBeNull = false)]
        public String reference { get; set; }

        [Column(CanBeNull = false)]
        public double amount { get; set; }

        [Column(CanBeNull = false)]
        public DateTime date { get; set; }

         [Column(CanBeNull = true)]
        public byte[] image { get; set; }
    }
}
