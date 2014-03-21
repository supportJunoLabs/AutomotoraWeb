using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class MenuOptionModel : AbstractModel {
        public string MenuName { get; set; }
        public string Level { get; set; }
        public string OpcionName { get; set; }
        public string Action { get; set; }
        public string Controlller { get; set; }
    }
}