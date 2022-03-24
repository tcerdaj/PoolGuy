using Xamarin.Forms;
using System.Linq;
using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using PoolGuy.Mobile.Data.Attributes;
using TypeSupport;
using TypeSupport.Extensions;
using PoolGuy.Mobile.CustomControls;

namespace PoolGuy.Mobile.Helpers
{
    public static class FieldValidationHelper
    {
        public static KeyValuePair<bool, string> IsFormValid(object model, Page page)
        {
            HideValidationFields(model, page);
            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, context, errors, true);
            if (!isValid)
            {
                ShowValidationFields(errors, model, page);
            }

            string error = string.Join(",", errors.Select(x => x.ErrorMessage).ToArray<string>());

            return new KeyValuePair<bool, string>(isValid, error);
        }
        private static void HideValidationFields
            (object model, Page page, string validationLabelSuffix = "Error")
        {
            if (model == null) { return; }
            var properties = GetValidatablePropertyNames(model);
            foreach (var propertyName in properties)
            {
                var errorControlName =
                $"{propertyName.Replace(".", "_")}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);
                if (control != null)
                {
                    control.Text?.Replace("\n", "");
                    control.IsVisible = false;
                }
            }
        }
        private static void ShowValidationFields
        (List<ValidationResult> errors,
        object model, Page page, string validationLabelSuffix = "Error")
        {
            if (model == null) { return; }
            foreach (var error in errors)
            {
                string memberName = string.Empty;
                if (error.GetType() == typeof(ValidationResult))
                {
                    memberName = $"{model.GetType().Name}_{error.MemberNames.FirstOrDefault()}";
                    memberName = memberName.Replace(".", "_");
                    var errorControlName = $"{memberName}{validationLabelSuffix}";
                    var control = page.FindByName<Label>(errorControlName);
                    if (control != null)
                    {
                        control.Text = $"{error.ErrorMessage}{Environment.NewLine}";
                        control.IsVisible = true;
                    }
                }
                else if (error.GetType() == typeof(CompositeValidationResult))
                {
                    var errorMember = error as CompositeValidationResult;

                    if (errorMember != null)
                    {
                        foreach (var result in errorMember.Results)
                        {
                            memberName = $"{model.GetType().Name}_{error.ErrorMessage}_{result.MemberNames.FirstOrDefault()}";
                            memberName = memberName.Replace(".", "_");
                            var errorControlName = $"{memberName}{validationLabelSuffix}";
                            var control = page.FindByName<Label>(errorControlName);
                            var entryControl = page.FindByName<CustomEntry>(errorControlName.Replace("Error","Entry"));
                            if (control != null && entryControl != null && string.IsNullOrEmpty(entryControl.Text))
                            {
                                control.Text = $"{result.ErrorMessage}{Environment.NewLine}";
                                control.IsVisible = true;
                            }

                        }    
                    }
                }
            }
        }
        private static IEnumerable<string> GetValidatablePropertyNames(object model)
        {
            var validatableProperties = new List<string>();
            var properties = GetValidatableProperties(model);
            foreach (var propertyInfo in properties)
            {
                
                if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType.Assembly.FullName == model.GetType().Assembly.FullName)
                {
                    // Adding reference fields
                    var referenceProperties = propertyInfo.PropertyType.GetProperties();
                    foreach (var reference in referenceProperties)
                    {
                        var errorControlName = $"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}.{reference.Name}";
                        validatableProperties.Add(errorControlName);
                    }
                }
                else
                {
                    var errorControlName = $"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}";
                    validatableProperties.Add(errorControlName);
                }
            }
            return validatableProperties;
        }
        private static List<PropertyInfo> GetValidatableProperties(object model)
        {
            var properties = model.GetType().GetProperties().Where(prop => prop.CanRead
                && prop.GetCustomAttributes(typeof(ValidationAttribute), true).Any()
                && prop.GetIndexParameters().Length == 0).ToList();
            return properties;
        }
    }
}