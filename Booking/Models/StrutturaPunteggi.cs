﻿namespace Booking
{
    using Booking.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class StrutturaPunteggi
    {
        public Struttura Struttura { get; set; }
        public double? MediaPunteggio { get; set; }
        public double? TotaleRecensioni { get; set; }
    }
}
