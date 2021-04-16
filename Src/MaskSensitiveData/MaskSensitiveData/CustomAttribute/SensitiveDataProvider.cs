using Newtonsoft.Json.Serialization;

namespace MaskSensitiveData
{
    public class SensitiveDataProvider : IValueProvider
    {
        private readonly string sensitiveDataMask;

        public SensitiveDataProvider(string mask)
        {
            sensitiveDataMask = mask;
        }

        public object GetValue(object target)
        {
            return sensitiveDataMask;
        }

        public void SetValue(object target, object value)
        {
            target = sensitiveDataMask;
        }
    }
}