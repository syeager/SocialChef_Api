using FluentValidation;
using FluentValidation.Results;

namespace SocialChef.Domain
{
    public class ModelConstructResult<T> where T : class
    {
        public T? Model { get; }
        public ValidationResult Validation { get; }

        public bool IsSuccess => Model != null && Validation.IsValid;

        private ModelConstructResult(T? model, ValidationResult validation)
        {
            Model = model;
            Validation = validation;
        }

        public static ModelConstructResult<T> Construct(T model, ValidationResult validation)
        {
            var modelResult = validation.IsValid ? model : null;
            return new ModelConstructResult<T>(modelResult, validation);
        }

        public T GetModelOrThrow()
        {
            if(!IsSuccess)
            {
                throw new ValidationException($"Validation failure for '{typeof(T)}`.", Validation.Errors);
            }

            return Model!;
        }
    }
}