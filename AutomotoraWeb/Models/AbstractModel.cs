using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public abstract class AbstractModel {

        public Int32 Id { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }

    }
}