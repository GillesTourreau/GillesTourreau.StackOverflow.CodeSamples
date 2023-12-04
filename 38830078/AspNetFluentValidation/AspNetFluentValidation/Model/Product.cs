//-----------------------------------------------------------------------
// <copyright file="Product.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace AspNetFluentValidation.Model
{
    public class Product
    {
        [JsonPropertyName("isLuxury")]
        public bool? IsPremium { get; set; }
    }
}
