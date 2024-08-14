using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Validators
{
    /// <summary>
    /// Model state validation attribute
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                foreach (var parameter in descriptor.MethodInfo.GetParameters())
                {
                    object obj = null;
                    if (context.ActionArguments.ContainsKey(parameter.Name))
                    {
                        obj = context.ActionArguments[parameter.Name];
                    }
                    ValidateAttributes(parameter, obj, context.ModelState);
                }
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
        private void ValidateAttributes(ParameterInfo parameter, object obj, ModelStateDictionary modelState)
        {
            foreach (var attributeData in parameter.CustomAttributes)
            {
                var attributeInstance = parameter.GetCustomAttribute(attributeData.AttributeType);
                if (attributeInstance is ValidationAttribute validationAttribute)
                {
                    var isValid = validationAttribute.IsValid(obj);
                    if (!isValid)
                    {
                        modelState.AddModelError("errors", $"Parameter '{parameter.Name}': {validationAttribute.FormatErrorMessage(parameter.Name)}");
                    }
                }
                else if (attributeInstance is FromBodyAttribute bindingAttribute)
                {
                    // event though objects can be mapped using FromQuery, 
                    // we expect to received them in body
                    // each QueryString should match to one action parameter
                    // This way it will be validated by the logic above
                    ValidateModel(parameter, obj, modelState);
                }
            }
        }
        /// <summary>
        /// Validate models provided in the Body of the request
        /// Models annotated with DataAnnotations Validation attributes will be validated
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="obj"></param>
        /// <param name="modelState"></param>
        private void ValidateModel(ParameterInfo parameter, object obj, ModelStateDictionary modelState)
        {
            if (obj == null)
                return;
            var validations = new List<ValidationResult>();
            var valCtx = new ValidationContext(obj, null, null);
            var isValid = Validator.TryValidateObject(obj, valCtx, validations);
            if (!isValid)
            {
                foreach (var error in validations)
                {
                    //modelState.AddModelError(parameter.Name, error.ErrorMessage);
                    modelState.AddModelError("errors", $"Parameter '{parameter.Name}': {error.ErrorMessage}");
                }
            }
        }
    }
}
