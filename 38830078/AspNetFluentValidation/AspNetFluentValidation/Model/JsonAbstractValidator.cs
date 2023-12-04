//-----------------------------------------------------------------------
// <copyright file="Class.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;

namespace AspNetFluentValidation.Model
{
    public abstract class JsonAbstractValidator<T> : AbstractValidator<T>
    {
        protected JsonAbstractValidator()
        {
        }

        protected override void OnRuleAdded(IValidationRule<T> rule)
        {
            var jsonPropertyPath = GetJsonPropertyPath(rule.Expression);

            rule.PropertyName = jsonPropertyPath;
            SetDisplayName(rule, jsonPropertyPath);

            base.OnRuleAdded(rule);
        }

        private static void SetDisplayName(IValidationRule<T> rule, string name)
        {
            // The SetDisplayName() method is not available in the IValidationRule<T>.
            // A feature request has been submitted on the GitHub of FluentValidation to avoid using reflection:
            // https://github.com/FluentValidation/FluentValidation/issues/2179
            var setDisplayNameMethod = rule.GetType().GetMethod("SetDisplayName", new[] { typeof(string) })!;

            setDisplayNameMethod.Invoke(rule, new object[] { name });
        }

        private static string GetJsonPropertyPath(LambdaExpression expression)
        {
            var body = expression.Body;

            if (body is UnaryExpression unaryExpression)
            {
                body = unaryExpression.Operand;
            }

            if (body is not MemberExpression memberExpression)
            {
                throw new ArgumentException("The argument is not expression to access to a property.", nameof(expression));
            }

            var path = new List<string>();

            while (true)
            {
                if (memberExpression.Member is not PropertyInfo property)
                {
                    throw new ArgumentException("The argument is not expression to access to a property.", nameof(expression));
                }

                path.Add(GetJsonPropertyName(property));

                if (memberExpression.Expression is ParameterExpression)
                {
                    break;
                }

                var nextMemberExpression = memberExpression.Expression as MemberExpression;

                if (nextMemberExpression is null)
                {
                    throw new ArgumentException("The argument is not expression to access to a property.", nameof(expression));
                }

                memberExpression = nextMemberExpression;
            }

            path.Reverse();

            return string.Join('.', path);
        }

        private static string GetJsonPropertyName(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();

            if (attribute is not null)
            {
                return attribute.Name;
            }

            return property.Name;
        }
    }
}
