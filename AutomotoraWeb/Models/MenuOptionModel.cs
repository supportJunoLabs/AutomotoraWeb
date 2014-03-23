using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Models {
    public class MenuOptionModel : AbstractModel {
        public string OpcionName { get; set; }
        public string Action { get; set; }
        public string Controlller { get; set; }
        public Dictionary<int, MenuOptionModel> SubMenu { get; set; }
    }
}