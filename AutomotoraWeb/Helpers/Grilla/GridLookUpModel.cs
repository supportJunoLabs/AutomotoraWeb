﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Helpers.Grilla {
    public class GridLookUpModel {
        public object GridLookUpCodigo { get; set; }
        public IEnumerable Opciones { get; set; }
    }
}