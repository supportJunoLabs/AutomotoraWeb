using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using AutomotoraWeb.Models;
using System.Collections;
using System.Web.Mvc;
using AutomotoraWeb.Helpers.Grilla;

namespace AutomotoraWeb.Utils {
    public class GeneralUtils {

        public static void ModelStateRemoveAllStarting(ModelStateDictionary ModelState, string sacar) {
            List<string> lsacar = new List<string>();
            foreach (string k in ModelState.Keys) {
                if (k.Length >= sacar.Length && k.Substring(0, sacar.Length).ToUpper().Equals(sacar.ToUpper())) {
                    lsacar.Add(k);
                }
            }
            foreach (var k in lsacar) {
                ModelState.Remove(k);
            }
        }



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