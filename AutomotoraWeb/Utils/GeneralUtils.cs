using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace AutomotoraWeb.Utils {
    public class GeneralUtils {

        public static bool isBoolean(PropertyInfo prop) { 
            if (prop.PropertyType.Name.Equals("Boolean")){
                return true;
            }
            if (Nullable.GetUnderlyingType(prop.PropertyType) != null &&
                Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("Boolean")) {
                return true;
            }
            return false;
        }

        public static bool isImporte(PropertyInfo prop) {
            if (prop.PropertyType.Name.Equals("Importe")) {
                return true;
            }
            return false;
        }

        public static bool isDateTime(PropertyInfo prop) { 
            if (prop.PropertyType.Name.Equals("DateTime") ){
                return true;
            } 
            if (Nullable.GetUnderlyingType(prop.PropertyType)!=null && 
                Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("DateTime")) {
                    return true;
            }
            return false;
        }

        public static bool isInteger(PropertyInfo prop) {
            if (prop.PropertyType.Name.Equals("Int32")) {
                return true;
            }
            if (Nullable.GetUnderlyingType(prop.PropertyType) != null &&
                Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("Int32")) {
                return true;
            }
            return false;
        }
    }
}