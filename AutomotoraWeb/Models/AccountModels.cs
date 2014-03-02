using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApuestasWeb.Models
{
    /// <summary>
    /// Funcion que valida el formato del e-mail segun los standares actuales
    /// </summary>
    public class EmailAttribute : RegularExpressionAttribute{
        public EmailAttribute(): base("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"){
        }
    }

    #region Models


    public class LogOnModel
    {
        [Required]
        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [DisplayName("Recordarme?")]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "El contraseña y la confirmación de la misma, no coinciden.")]
    public class RegisterModelInicial
    {

        [Required(ErrorMessage = "Usuario Incorrecto")]
        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Correo Electrónico")]
        [Email(ErrorMessage = "Ingrese un correo válido")]        
        public string Email { get; set; }


        [Required(ErrorMessage = "Contraseña no valida")]
        [ValidatePasswordLength(ErrorMessage = "Largo debe ser mayor a 8")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repita su Contraseña")]
        [ValidatePasswordLength(ErrorMessage = "Largo debe ser mayor a 8")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Fecha Inválida")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Nacimiento")]
        public DateTime FechaNac { get; set; }

        [Required(ErrorMessage = "Seleccione un sexo")]
        [DataType(DataType.Text)]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [Required]
        [BooleanMustBeTrue(ErrorMessage = "Debe Aceptar los Términos y Condiciones Básicas")]
        [DisplayName("Acepta Términos y Condiciones Básicas?")]
        public bool AceptaTerminosContratoBasicas { get; set; }

    }

    //---------------------------------------------------------------------------------------------------
    
    
    
    /// <summary>
    ///Funcion que se usa para validar en el Model que los checkbox vengan chequeados
    ///obligatoriamente o sea value de checkbox = true
    /// </summary>
            
    public class BooleanMustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object propertyValue)
        {
            return propertyValue != null
                && propertyValue is bool
                && (bool)propertyValue;
        }
    }


    //---------------------------------------------------------------------------------------------------

    public class RegisterModel
    {

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Tipo Documento")]
        public string TipoDocumento { get; set; }

        [Required(ErrorMessage = "Documento inválido")]
        [StringLength(9, ErrorMessage = "Largo de documento no válido")]
        [DataType(DataType.Text)]
        [DisplayName("Documento")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Ciudad")]
        public string Ciudad { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Código Postal")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        //[Required(ErrorMessage = "Obligatorio")]
        //[DataType(DataType.Text)]
        //[DisplayName("Pais")]
        //public string Pais { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Pais")]
        public string Pais { get; set; } 

        [Required]
        [BooleanMustBeTrue(ErrorMessage = "Debe Aceptar")]
        [DisplayName("Acepta Términos y Condiciones?")]
        public bool AceptaTerminosContrato { get; set; }
    }
    #endregion

    //---------------------------------------------------------------------------------------------------

    //<!-- 5/3/12 -->

    public class CambiarDatosModel
    {

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DisplayName("Correo Electrónico")]
        [Email(ErrorMessage = "Ingrese un correo válido")]        
        public string Email { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Ciudad")]
        public string Ciudad { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Código Postal")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Text)]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Fecha Inválida")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Nacieminto")]
        public DateTime FechaNac { get; set; }

        [Required(ErrorMessage = "Seleccione un sexo")]
        [DataType(DataType.Text)]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }


    }
    //<!-- 5/3/12 -->
    //---------------------------------------------------------------------------------------------------

    public class CuentaPIN
    {
        [Required(ErrorMessage = "Cuenta no valida")]
        [DataType(DataType.Text)]
        [DisplayName("Numero de Cuenta")]
        public string Cuenta { get; set; }

        [Required(ErrorMessage = "PIN incorrecto")]
        [DataType(DataType.Password)]
        [DisplayName("PIN")]
        public string PIN { get; set; }
    }

    //<!-- LEO 06/07/2012 -->
    public class FinalizarRegistro{
    
    }
    //<!-- LEO 06/07/2012 -->


     
    public class EstadoDeCuentaModel
    {
        
        [Required(ErrorMessage = "Fecha Inválida")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "Fecha Inválida")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        public DateTime FechaHasta { get; set; }        
        
        [DisplayName("Incluir Tarjetas")]
        public bool incluirTarjetas { get; set; }

        [DisplayName("Detalle Apuestas")]
        public bool detalleApuestas { get; set; }

    }

    //Reyna 
    public class ReasignacionCuentaModel
    {
        [Required]
        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }
    }

    //---------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    //---------------------------------------------------------------------------------------------------

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    //---------------------------------------------------------------------------------------------------

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Campo no puede esta vacio.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Campo no puede esta vacio.", "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Campo no puede esta vacio.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Campo no puede esta vacio.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Campo no puede esta vacio.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Campo no puede esta vacio.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Campo no puede esta vacio.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Campo no puede esta vacio.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    //---------------------------------------------------------------------------------------------------

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Campo no puede esta vacio.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    //---------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Nombre de usuario ya existe. Seleccione un nombre diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe un usuario con ese correo electrónico. Seleccione un nombre diferente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Password invalido.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Correo electrónico invalido.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Nombre de usuario invalido.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    //---------------------------------------------------------------------------------------------------

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    //---------------------------------------------------------------------------------------------------

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' debe tener al menos {1} caracteres.";
        private readonly int _minCharacters = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("MinPasswordLength")); //Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

}
