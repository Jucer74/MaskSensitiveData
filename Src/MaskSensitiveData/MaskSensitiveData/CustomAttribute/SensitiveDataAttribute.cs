using System;

namespace MaskSensitiveData
{
    /// <summary>
    /// Allow assign mask to the fields marked as sensitive data
    /// </summary>
    public sealed class SensitiveDataAttribute : Attribute
    {
        private const string DEFAULT_MASK = "**********";

        public SensitiveDataAttribute(string mask = DEFAULT_MASK)
        {
            Mask = mask;
        }

        public string Mask { get; }
    }
}