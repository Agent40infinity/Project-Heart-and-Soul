namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using UnityEngine;

    /// <summary>
    ///   Inverts a number data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Operations/[DB] Invert Number Operation")]
    public class InvertNumberOperation : InvertOperation<float>
    {
        /// <inheritdoc />
        protected override float Invert(float value)
        {
            return value * -1;
        }
    }
}