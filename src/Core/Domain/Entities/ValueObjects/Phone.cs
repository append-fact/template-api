﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects
{
    public class Phone
    {
        public string? PhoneAreaCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
