using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MaskSensitiveData.CustomSerializer
{
    /// <summary>
    /// Class to Protect Data marked as Sensitive
    /// </summary>
    /// <Remarks>
    /// Based on: https://github.com/ThiagoBarradas/jsonmasking
    /// </Remarks>
    public static class Protector
    {
        #region Constants

        private static readonly string ARRAY_DEFINITION_REGEXP = @"\[[0-9]*\]";

        #endregion Constants

        /// <summary>
        /// Identify the Fields marked by the attribute [SensitiveData] and replace the value for the Mask
        /// </summary>
        /// <typeparam name="TData">Type of data to review and protect</typeparam>
        /// <param name="data">Object with the values to protect</param>
        /// <returns>Return the Json serialized with the data protected</returns>
        public static string MaskFields<TData>(TData data)
        {
            var serializedData = JsonConvert.SerializeObject(data);

            return MaskFields(serializedData, typeof(TData));
        }

        /// <summary>
        /// Receive the serialized data and the dataType to convert
        /// </summary>
        /// <param name="serializedData">the Json data</param>
        /// <param name="dataType">The Class or data type to get sensitive data information</param>
        /// <returns>Return the Json serialized with the data protected</returns>
        public static string MaskFields(string serializedData, Type dataType)
        {
            var maskedData = serializedData;

            Dictionary<string, string> maskedFieldsDic = new Dictionary<string, string>();
            GetMaskedFields(dataType, maskedFieldsDic);

            if (maskedFieldsDic.Count > 0)
            {
                // Convert to JSon Object
                var jsonObject = (JObject)JsonConvert.DeserializeObject(serializedData);

                // Iterate over the Object and replace the values with the mask
                MaskFieldsFromJToken(jsonObject, maskedFieldsDic);
                maskedData = jsonObject.ToString();
            }

            return maskedData;
        }

        /// <summary>
        /// Clean the Array definition for the JSonProperty
        ///
        /// For example:
        ///
        /// if we have a Json with the below structure
        ///
        /// {
        ///   "Id": 1,
        ///   "FirstName": "John",
        ///   "LastName": "Doe",
        ///   "DateOfBirth": "1974-10-08T00:00:00",
        ///   "PersonIdentifier": "765a7458-dcc7-481a-8d56-368ba9100059",
        ///   "Jobs": [
        ///     {
        ///       "Id": 1,
        ///       "Company": "International Company",
        ///       "StartDate": "1991-01-13T00:00:00",
        ///       "EndDate": null,
        ///       "IsCurrent": true,
        ///       "Positions": [
        ///         {
        ///           "Id": 1,
        ///           "Name": "System Engineer",
        ///           "StartDate": "1991-01-13T00:00:00",
        ///           "EndDate": null,
        ///           "Salary": 7000,
        ///           "IsCurrent": true
        ///         }
        ///       ]
        ///     }
        ///   ]
        /// }
        ///
        /// For some of the fields within Jobs, the property path will be:
        ///
        /// - Jobs[0].Id
        /// . Jobs[0].Positions[0].Salary
        ///
        /// Then, to allow identifying the properties inside,
        /// regardless of the Position of the Array,
        /// we clean the definition of the Array, like this:
        ///
        /// - Jobs[0].Id ==> Jobs.Id
        /// - Jobs[0].Positions[0].Salary ==> Jobs.Positions.Salary
        ///
        /// This result value must be equal to the Dictionary key,
        /// is this field is Sensitive Data.
        /// </summary>
        /// <param name="propPath">Json Property Path</param>
        /// <returns>Property path with out Array Definition</returns>
        private static string CleanPropertyPath(string propPath)
        {
            return Regex.Replace(propPath, ARRAY_DEFINITION_REGEXP, string.Empty);
        }

        /// <summary>
        /// Iterate over the Fields into the Structure object data and identify the Fields
        /// marked by the SensitiveData Attribute
        /// </summary>
        /// <param name="dataType">The data type or class with the sensitive data</param>
        /// <param name="data">Object with the data</param>
        /// <param name="maskedFieldsDic">Dictionary to store the Fields marked as Sensitive Data and the mask</param>
        /// <param name="propPath">Property path realated to the object structure
        ///
        /// For example, if we have the below objects structure:
        ///
        /// public class PersonData
        /// {
        ///     public int Id { get; set; }
        ///
        ///     [SensitiveData]
        ///     public string FirstName { get; set; }
        ///
        ///     [SensitiveData]
        ///     public string LastName { get; set; }
        ///
        ///     [SensitiveData("YYYY-MM-DD")]
        ///     public DateTime DateOfBirth { get; set; }
        ///
        ///     [SensitiveData("99999999-9999-9999-9999-999999999999")]
        ///     public Guid PersonIdentifier { get; set; }
        ///
        ///     public IList<Job> Jobs { get; set; }
        /// }
        ///
        /// public class Job
        /// {
        ///     public int Id { get; set; }
        ///
        ///     [SensitiveData("AAAAAAAAAA")]
        ///     public string Company { get; set; }
        ///
        ///     public DateTime StartDate { get; set; }
        ///     public DateTime? EndDate { get; set; }
        ///     public bool IsCurrent { get; set; }
        ///     public IList<Position> Positions { get; set; }
        /// }
        ///
        /// public class Position
        /// {
        ///     public int Id { get; set; }
        ///     public string Name { get; set; }
        ///     public DateTime StartDate { get; set; }
        ///     public DateTime? EndDate { get; set; }
        ///
        ///     [SensitiveData("##########")]
        ///     public Decimal Salary { get; set; }
        ///
        ///     public bool IsCurrent { get; set; }
        /// }
        ///
        /// At the end of the recursive interation we retrieve the below dictionary data:
        ///
        ///     PropertyPath            Mask
        ///     ---------------------   ------------------------------------
        ///     FirstName               **********
        ///     LastNames               **********
        ///     DateOfBirth             YYYY-MM-DD
        ///     PersonIdentifier        99999999-9999-9999-9999-999999999999
        ///     Jobs.Company            AAAAAAAAAA
        ///     Jobs.Positions.Salary   ##########
        ///
        /// The Fields are relative of inside PersonData ,
        /// then the initial property path doesn't include the PersonData name
        ///
        /// </param>
        /// <Remarks>
        /// The Generic fields supported are only IList types.
        /// </Remarks>
        private static void GetMaskedFields(Type dataType, Dictionary<string, string> maskedFieldsDic, string propPath = null)
        {
            PropertyInfo[] propertiesInfo = dataType.GetProperties();

            foreach (var prop in propertiesInfo)
            {
                var isMasked = Attribute.IsDefined(prop, typeof(SensitiveDataAttribute));
                if (isMasked)
                {
                    SensitiveDataAttribute SensitiveDataAttribute = (SensitiveDataAttribute)Attribute.GetCustomAttribute(prop, typeof(SensitiveDataAttribute));
                    maskedFieldsDic.Add((propPath is null) ? prop.Name : $"{propPath}.{prop.Name}", SensitiveDataAttribute.Mask);
                }

                if (!IsBaseType(prop))
                {
                    if (IsGenericType(prop))
                    {
                        GetMaskedFields(prop.PropertyType.GenericTypeArguments[0], maskedFieldsDic, (propPath is null) ? prop.Name : $"{propPath}.{prop.Name}");
                    }
                    else
                    {
                        if (IsClassType(prop))
                        {
                            GetMaskedFields(prop.PropertyType, maskedFieldsDic, prop.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validate if the property  is a basic or base type like:
        /// - int, long, double, decimal, float, short, byte or a string or object type
        /// </summary>
        /// <param name="propertyInfo">Property Information</param>
        /// <returns>True if the property is a base type, otherwise False.</returns>
        private static bool IsBaseType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsValueType;
        }

        /// <summary>
        /// Validate if the property is a class different to String and Object
        /// </summary>
        /// <param name="propertyInfo">Property Information</param>
        /// <returns>True if the property is a class type, otherwise False.</returns>
        private static bool IsClassType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsClass;
        }

        /// <summary>
        /// Validate if the property is a generic Object
        /// </summary>
        /// <param name="propertyInfo">Property Information</param>
        /// <returns>True if the property is a generic type, otherwise False.</returns>
        private static bool IsGenericType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsGenericType;
        }

        /// <summary>
        /// Replace the JsonToken Value with the masked related with the field
        /// </summary>
        /// <param name="token">Json Token</param>
        /// <param name="maskedFields">Masked Fields masked as Sensitive Data in the Object Structure</param>
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