﻿using Newtonsoft.Json;
using PromisePayDotNet.Internals;

namespace PromisePayDotNet.Enums
{
    /// <summary>
    /// Who pays the fee (buyer, seller, cc, int_wire)
    /// </summary>
    [JsonConverter(typeof(FeeToJsonConverter))]
    public enum FeeToType
    {
        /// <summary>
        /// Default value
        /// </summary>
        None,
        /// <summary>
        /// The fee is paid by the buyer
        /// </summary>
        Buyer,
        /// <summary>
        /// The fee is paid by the seller
        /// </summary>
        Seller,
        CC,
        IntWire,
        PaypalPayout // 
    }
}