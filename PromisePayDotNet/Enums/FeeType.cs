namespace PromisePayDotNet.Enums
{
    /// <summary>
    /// Fee type.
    /// </summary>
    public enum FeeType
    {
        /// <summary>
        /// Fixed fee type == 1.
        /// </summary>
        Fixed = 1,
        /// <summary>
        /// Percentage type == 2.
        /// </summary>
        Percentage = 2,
        /// <summary>
        /// Percentage with cap == 3.
        /// </summary>
        PercentageWithCap = 3,
        /// <summary>
        /// Percentage with minimum == 4.
        /// </summary>
        PercentageWithMin = 4
    }
}
