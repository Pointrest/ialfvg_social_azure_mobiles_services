﻿using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baassiService.DataObjects
{
    public class Post : EntityData
    {
        public string Text { get; set; }
    }
}
