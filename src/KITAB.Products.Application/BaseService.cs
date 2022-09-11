using FluentValidation;
using FluentValidation.Results;
using KITAB.Products.Application.Notificators;
using KITAB.Products.Domain.Models;

namespace KITAB.Products.Application
{
    public abstract class BaseService
    {
        private readonly INotificatorService _notificatorService;

        protected BaseService(INotificatorService notificatorService)
        {
            _notificatorService = notificatorService;
        }

        protected void Notify(ValidationResult p_validationResult)
        {
            foreach (var error in p_validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string p_message)
        {
            // Propaga o erro até a camada de apresentação
            _notificatorService.Handle(new Notification(p_message));
        }

        protected bool RunValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : class
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}
