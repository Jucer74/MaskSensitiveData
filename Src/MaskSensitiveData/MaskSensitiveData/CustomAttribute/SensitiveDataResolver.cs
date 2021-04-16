using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace MaskSensitiveData
{
    public class SensitiveDataResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member is PropertyInfo)
            {
                var propertyInfo = (PropertyInfo)member;
                var isSensitiveData = Attribute.IsDefined(propertyInfo, typeof(SensitiveDataAttribute));

                if (isSensitiveData)
                {
                    if (propertyInfo.PropertyType.Name.Equals(nameof(String)))
                    {
                        SensitiveDataAttribute sensitiveAttribute = (SensitiveDataAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(SensitiveDataAttribute));
                        string mask = sensitiveAttribute.Mask;
                        property.ValueProvider = new SensitiveDataProvider(mask);
                    }
                }
            }

            return property;
        }
    }
}