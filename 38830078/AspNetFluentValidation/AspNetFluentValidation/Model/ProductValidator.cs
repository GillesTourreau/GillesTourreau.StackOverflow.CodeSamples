//-----------------------------------------------------------------------
// <copyright file="ProductValidator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using FluentValidation;

namespace AspNetFluentValidation.Model
{
    public class ProductValidator : JsonAbstractValidator<Product>
    {
        public ProductValidator()
        {
            this.RuleFor(product => product.IsPremium).NotNull();
        }
    }
}
