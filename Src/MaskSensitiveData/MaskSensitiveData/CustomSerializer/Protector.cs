using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MaskSensitiveData.CustomSerializer
{
    /// <summary>
    /// Based on:
    /// https://github.com/ThiagoBarradas/jsonmasking
    /// </summary>
    public static class Protector
    {
        public static string MaskFields<TData>(TData data)
        {
            var serializedData = JsonConvert.SerializeObject(data);

            Dictionary<string, string> maskedFieldsDic = new Dictionary<string, string>();
            GetMaskedFields(typeof(TData), maskedFieldsDic);

            if (maskedFieldsDic.Count >= 1)
            {
                // Convert to JSon Object
                var jsonObject = (JObject)JsonConvert.DeserializeObject(serializedData);

                // Iterate over the Object and replace the values with the mask
                MaskFieldsFromJToken(jsonObject, maskedFieldsDic);
                serializedData = jsonObject.ToString();
            }

            return serializedData;
        }

        public static string MaskFields(string serializedData, Type dataType)
        {
            var maskedData = serializedData;

            Dictionary<string, string> maskedFieldsDic = new Dictionary<string, string>();
            GetMaskedFields(dataType, maskedFieldsDic);

            if (maskedFieldsDic.Count >= 1)
            {
                // Convert to JSon Object
                var jsonObject = (JObject)JsonConvert.DeserializeObject(serializedData);

                // Iterate over the Object and replace the values with the mask
                MaskFieldsFromJToken(jsonObject, maskedFieldsDic);
                maskedData = jsonObject.ToString();
            }

            return maskedData;
        }

        private static string CleanPropertyPath(string propPath)
        {
            return Regex.Replace(propPath, @"\[[0-9]*\]", string.Empty);
        }

        private static void GetMaskedFields(Type dataType, Dictionary<string, string> maskedFieldsDic, string path = null)
        {
            PropertyInfo[] propertiesInfo = dataType.GetProperties();

            foreach (var prop in propertiesInfo)
            {
                var isMasked = Attribute.IsDefined(prop, typeof(SensitiveDataAttribute));
                if (isMasked)
                {
                    SensitiveDataAttribute SensitiveDataAttribute = (SensitiveDataAttribute)Attribute.GetCustomAttribute(prop, typeof(SensitiveDataAttribute));
                    maskedFieldsDic.Add((path is null) ? prop.Name : $"{path}.{prop.Name}", SensitiveDataAttribute.Mask);
                }

                if (!IsBaseType(prop))
                {
                    if (prop.PropertyType.IsGenericType)
                    {
                        if (prop.PropertyType.GenericTypeArguments.Length > 0)
                        {
                            GetMaskedFields(prop.PropertyType.GenericTypeArguments[0], maskedFieldsDic, (path is null) ? prop.Name : $"{path}.{prop.Name}");
                        }
                    }
                    else
                    {
                        if (prop.PropertyType.IsClass)
                        {
                            GetMaskedFields(prop.PropertyType, maskedFieldsDic, (path is null) ? prop.Name : $"{path}.{prop.Name}");
                        }
                    }
                }
            }
        }

        private static bool IsBaseType(PropertyInfo propertyInfo)
        {
            return (propertyInfo.PropertyType.IsValueType ||
                propertyInfo.PropertyType.FullName.Equals("System.String") ||
                propertyInfo.PropertyType.FullName.Equals("System.Object"));
        }

        private static void MaskFieldsFromJToken(JToken token, Dictionary<string, string> maskedFields)
        {
            if (!(token is JContainer container))
            {
                return; // abort recursive
            }

            List<JToken> blackList = new List<JToken>();
            foreach (JToken jtoken in container.Children())
            {
                if (jtoken is JProperty prop)
                {
                    var matching = maskedFields.ContainsKey(CleanPropertyPath(prop.Path));

                    if (matching)
                    {
                        blackList.Add(jtoken);
                    }
                }
                // Recursive call to internal Node Value
                MaskFieldsFromJToken(jtoken, maskedFields);
            }

            // replace
            foreach (JToken tok in blackList)
            {
                var prop = (JProperty)tok;
                prop.Value = maskedFields[CleanPropertyPath(prop.Path)];
            }
        }
    }
}