using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation.Validators;

namespace WebAppCore.BLL.Validations
{
    public class UniquePropertyValidator<T> : PropertyValidator where T : class
    {
        private readonly IEnumerable<T> _items;

        public UniquePropertyValidator(IEnumerable<T> items) : base("{PropertyName} must be unique")
        {
            _items = items;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var editedItem = context.Instance as T;
            var newValue = context.PropertyValue as string;
            var property = typeof(T).GetTypeInfo().GetDeclaredProperty(context.PropertyName);
            return _items.All(item => item.Equals(editedItem) || property.GetValue(item).ToString() != newValue);
        }
    }
}
