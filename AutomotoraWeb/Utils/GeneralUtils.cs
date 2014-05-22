using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using AutomotoraWeb.Models;

namespace AutomotoraWeb.Utils {
    public class GeneralUtils {

        public static bool isBoolean(PropertyInfo prop, ColumnaGrilla cg) {

            if (prop != null) {

                if (prop.PropertyType.Name.Equals("Boolean")) {
                    return true;
                }
                if (Nullable.GetUnderlyingType(prop.PropertyType) != null &&
                    Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("Boolean")) {
                    return true;
                }
            }
            return cg.EsBoolean;
        }

        public static bool isImporte(PropertyInfo prop, ColumnaGrilla cg) {
            if (prop != null) {
                if (prop.PropertyType.Name.Equals("Importe")) {
                    return true;
                }
            }
            return cg.EsImporte;
        }

        public static bool isDateTime(PropertyInfo prop, ColumnaGrilla cg) {
            if (prop != null) {
                if (prop.PropertyType.Name.Equals("DateTime")) {
                    return true;
                }
                if (Nullable.GetUnderlyingType(prop.PropertyType) != null &&
                    Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("DateTime")) {
                    return true;
                }
            }
            return cg.EsFecha;
        }

        public static bool isInteger(PropertyInfo prop, ColumnaGrilla cg) {
            if (prop != null) {
                if (prop.PropertyType.Name.Equals("Int32")) {
                    return true;
                }
                if (Nullable.GetUnderlyingType(prop.PropertyType) != null &&
                    Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("Int32")) {
                    return true;
                }
            }
            return cg.EsEntero;
        }
    }
}