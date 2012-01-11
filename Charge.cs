﻿using System;
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
    [Table (Name="Charge")]
   
    public class Charge
    {
        [Association(Storage = "ReferencesCharge", OtherKey = "chargeId")]
        public EntitySet<ReferencesCharge> ReferencesCharge { get; set; }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int chargeId { get; set;}
        
        [Column(CanBeNull = false)]
        public double amount { get; set; }

        [Column(CanBeNull = false)]
        public String phone { get; set; }
        
        [Column(CanBeNull = false)]
        public String person { get; set; }
        
        [Column(CanBeNull = false)]
        public int NumberReferences { get; set; }
        
        [Column(CanBeNull = true)]
        public String description { get; set; }
    }

    
}
