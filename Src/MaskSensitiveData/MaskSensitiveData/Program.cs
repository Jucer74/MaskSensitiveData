using MaskSensitiveData.CustomSerializer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MaskSensitiveData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //////////////////////////////////////////////////
            //// Data To Serialize
            //////////////////////////////////////////////////

            #region Data

            // Simple Data

            #region Simple-Data

            var simpleData = new SimpleData()
            {
                PersonalId = 1,
                PersonalName = "My Name",
                Password = "My Password",
                CreditCardNumber = "532478573156543",
                PersonalIdentifier = Guid.Parse("D7B2D998-9723-47B0-8588-D855E505B718"),
                CreatedDate = DateTime.Now
            };

            #endregion Simple-Data

            // Complex Data

            #region Complex-Data

            var complexData = new ComplexData()
            {
                Id = 1,
                FirstName = "Julio",
                LastName = "Robles",
                DateOfBirth = DateTime.Parse("1974-10-08"),
                PersonIdentifier = Guid.Parse("765A7458-DCC7-481A-8D56-368BA9100059"),
                IsActive = true,
                CurrentPosition = new Position()
                {
                    Id = 124,
                    Name = "Solutions Architect",
                    Salary = 9000,
                    StartDate = DateTime.Parse("1992-06-10"),
                    EndDate = null,
                    IsCurrent = true
                },
                Jobs = new List<Job>()
                {
                    new Job()
                    {
                        Id= 1,
                        Company = "Open International",
                        StartDate = DateTime.Parse("1991-01-13"),
                        EndDate = DateTime.Parse("1992-06-02"),
                        IsCurrent = false,
                        Positions = new List<Position>()
                        {
                            new Position()
                            {
                                Id = 123,
                                Name = "System Engineer",
                                Salary = 7000,
                                StartDate = DateTime.Parse("1991-01-13"),
                                EndDate = DateTime.Parse("1992-06-02"),
                                IsCurrent = false
                            }
                        }
                    },
                    new Job()
                    {
                        Id= 2,
                        Company = "Perficient Latam",
                        StartDate = DateTime.Parse("1992-06-10"),
                        EndDate = null,
                        IsCurrent = true,
                        Positions = new List<Position>()
                        {
                            new Position()
                            {
                                Id = 124,
                                Name = "Solutions Architect",
                                Salary = 9000,
                                StartDate = DateTime.Parse("1992-06-10"),
                                EndDate = null,
                                IsCurrent = true
                            }
                        }
                    },
                }
            };

            #endregion Complex-Data

            #endregion Data

            //////////////////////////////////////////////////
            //// Normal Serialization
            //////////////////////////////////////////////////

            //#region Normal-Serialization

            //Console.WriteLine("Normal Serialization");
            //Console.WriteLine("==================== ");
            //Console.WriteLine();

            //// Act
            //var simpleDataNormalSerialized = JsonConvert.SerializeObject(simpleData, Formatting.Indented);
            //var complexDataNormalSerialized = JsonConvert.SerializeObject(complexData, Formatting.Indented);

            //// Assert
            //Console.WriteLine("Simple Data");
            //Console.WriteLine("-----------");
            //Console.WriteLine(simpleDataNormalSerialized);
            //Console.WriteLine();
            //Console.WriteLine("Complex Data");
            //Console.WriteLine("------------");
            //Console.WriteLine(complexDataNormalSerialized);
            //Console.WriteLine();
            //Console.WriteLine("... Press any key to continue ,,,");
            //Console.ReadKey();
            //Console.Clear();

            //#endregion Normal-Serialization

            //////////////////////////////////////////////////
            //// Using Sensitive Attribute - Contract Resolver
            //////////////////////////////////////////////////

            //#region Sensitive-Attribute

            //Console.WriteLine("Sensitive-Attribute");
            //Console.WriteLine("===================");

            //// Act
            //var settings = new JsonSerializerSettings() { ContractResolver = new SensitiveDataResolver() };
            //var simpleDataContractResolver = JsonConvert.SerializeObject(simpleData, Formatting.Indented, settings);
            //var complexDataContractResolver = JsonConvert.SerializeObject(complexData, Formatting.Indented, settings);

            //// Assert
            //Console.WriteLine("Simple Data");
            //Console.WriteLine("-----------");
            //Console.WriteLine(simpleDataContractResolver);
            //Console.WriteLine();
            //Console.WriteLine("Complex Data");
            //Console.WriteLine("------------");
            //Console.WriteLine(complexDataContractResolver);
            //Console.WriteLine();
            //Console.WriteLine("... Press any key to continue ,,,");
            //Console.ReadKey();
            //Console.Clear();

            //#endregion Sensitive-Attribute

            //////////////////////////////////////////////////
            //// Using JSonConverter
            //////////////////////////////////////////////////

            //#region JSonConverter

            //Console.WriteLine("Using JSonConverter");
            //Console.WriteLine("===================");
            //// Arrange
            //var simpleInfo = new SimpleInfo()
            //{
            //    PersonalId = 1,
            //    PersonalName = "My Name",
            //    Password = "My Password",
            //    CreditCardNumber = "532478573156543",
            //    PersonalIdentifier = Guid.Parse("D7B2D998-9723-47B0-8588-D855E505B718"),
            //    CreatedDate = DateTime.Now
            //};

            //var complexInfo = new ComplexInfo()
            //{
            //    CustomerId = 1,
            //    CustomerName = "Julio Robles",
            //    CustomerIdentifier = Guid.Parse("2C537207-8178-41A1-91D2-15D94727762F"),
            //    DateOfBirth = DateTime.Parse("1974-10-08"),
            //    IsActive = true,
            //    Password = "MyP4ssw0rd",
            //    Payments = new List<PaymentMethod>()
            //    {
            //        new PaymentMethod()
            //        {
            //            TransactionDate = DateTime.Parse("2021-01-13T10:20:30"),
            //            PaymentType = PaymentType.Cash
            //        },
            //        new PaymentMethod()
            //        {
            //            TransactionDate = DateTime.Parse("2021-03-16T20:10:00"),
            //            PaymentType = PaymentType.CreditCard,
            //            CreditCardNumber = "532478573156543",
            //            CreditCardExpirationDate = DateTime.Parse("2025-10-10")
            //        }
            //    }
            //};

            //// Act
            //var simpleDataJsonConvert = JsonConvert.SerializeObject(simpleInfo, Formatting.Indented);
            //var complexInfoJsonConvert = JsonConvert.SerializeObject(complexInfo, Formatting.Indented);

            //// Assert
            //Console.WriteLine("Simple Data");
            //Console.WriteLine("-----------");
            //Console.WriteLine(simpleDataJsonConvert);
            //Console.WriteLine();
            //Console.WriteLine("Complex Data");
            //Console.WriteLine("------------");
            //Console.WriteLine(complexInfoJsonConvert);
            //Console.WriteLine();
            //Console.WriteLine("... Press any key to continue ...");
            //Console.ReadKey();
            //Console.Clear();

            //#endregion JSonConverter

            //////////////////////////////////////////////////
            //// Using thirdParty Library (JsonMasking)
            //////////////////////////////////////////////////

            //#region ThirdParty Library: JsonMasking

            //Console.WriteLine("ThirdParty Library: JsonMasking");
            //Console.WriteLine("===============================");

            //var simpleJson = JsonConvert.SerializeObject(simpleData, Formatting.Indented); // value must be a json string to masked
            //var complexJson = JsonConvert.SerializeObject(complexData, Formatting.Indented); // value must be a json string to masked

            //// note that password is only replaced when is in root path
            //var blacklist = new string[] { "Password", "*Creditcardnumber", "PersonIdentifier", "PersonalIdentifier", "IsActive", "CreatedDate", "*IsCurrent", "*Salary" };
            //var mask = "******";

            //// Act
            //var simpleJsonMasked = JsonMasking.JsonMasking.MaskFields(simpleJson, blacklist, mask);
            //var complexJsonMasked = JsonMasking.JsonMasking.MaskFields(complexJson, blacklist, mask);

            //// Assert
            //Console.WriteLine("Simple Data");
            //Console.WriteLine("-----------");
            //Console.WriteLine(simpleJsonMasked);
            //Console.WriteLine();
            //Console.WriteLine("Complex Data");
            //Console.WriteLine("------------");
            //Console.WriteLine(complexJsonMasked);
            //Console.WriteLine();
            //Console.WriteLine("... Press any key to continue ...");
            //Console.ReadKey();
            //Console.Clear();

            //#endregion ThirdParty Library: JsonMasking

            //////////////////////////////////////////////////
            //// Protector Masking (Based on JsonMasking)
            //////////////////////////////////////////////////

            //#region Protector

            //Console.WriteLine("Custom Protector");
            //Console.WriteLine("================");

            //// Act
            //var simpleDataMasked = Protector.MaskFields(simpleData);
            //var complexDataMasked = Protector.MaskFields(complexData);

            //// Assert
            //Console.WriteLine("Simple Data");
            //Console.WriteLine("-----------");
            //Console.WriteLine(simpleDataMasked);
            //Console.WriteLine();
            //Console.WriteLine("Complex Data");
            //Console.WriteLine("------------");
            //Console.WriteLine(complexDataMasked);
            //Console.WriteLine();
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
            //Console.Clear();

            //#endregion Protector

            ////////////////////////////////////////////////
            // Protector Masking - typeOf
            ////////////////////////////////////////////////

            #region Protector-typeof

            Console.WriteLine("Custom Protector - typeof");
            Console.WriteLine("=========================");
            // Act
            var serializedSimpleData = JsonConvert.SerializeObject(simpleData);
            //var serializedSimpleData = "{\"personalId\":1,\"personalName\":\"My Name\",\"password\":\"My Password\",\"creditCardNumber\":\"532478573156543\",\"personalIdentifier\":\"d7b2d998-9723-47b0-8588-d855e505b718\",\"createdDate\":\"2021-06-04T23:11:31.384422-05:00\",\"isActive\":false}";
            var simpleMasked = Protector.MaskFields(serializedSimpleData, typeof(SimpleData));

            var serializedComplexData = JsonConvert.SerializeObject(complexData);
            //var serializedComplexData = "{\"id\":1,\"firstName\":\"Julio\",\"lastName\":\"Robles\",\"dateOfBirth\":\"1974-10-08T00:00:00\",\"personIdentifier\":\"765a7458-dcc7-481a-8d56-368ba9100059\",\"isActive\":true,\"currentPosition\":{\"id\":124,\"name\":\"Solutions Architect\",\"startDate\":\"1992-06-10T00:00:00\",\"endDate\":null,\"salary\":9000.0,\"isCurrent\":true},\"jobs\":[{\"id\":1,\"company\":\"Open International\",\"startDate\":\"1991-01-13T00:00:00\",\"endDate\":\"1992-06-02T00:00:00\",\"isCurrent\":false,\"positions\":[{\"id\":123,\"name\":\"System Engineer\",\"startDate\":\"1991-01-13T00:00:00\",\"endDate\":\"1992-06-02T00:00:00\",\"salary\":7000.0,\"isCurrent\":false}]},{\"id\":2,\"company\":\"Perficient Latam\",\"startDate\":\"1992-06-10T00:00:00\",\"endDate\":null,\"isCurrent\":true,\"positions\":[{\"id\":124,\"name\":\"Solutions Architect\",\"startDate\":\"1992-06-10T00:00:00\",\"endDate\":null,\"salary\":9000.0,\"isCurrent\":true}]}]}";
            var complexMasked = Protector.MaskFields(serializedComplexData, typeof(ComplexData));

            // Assert
            Console.WriteLine("Simple Data");
            Console.WriteLine("-----------");
            Console.WriteLine(simpleMasked);
            Console.WriteLine();
            Console.WriteLine("Complex Data");
            Console.WriteLine("------------");
            Console.WriteLine(complexMasked);
            Console.WriteLine();
            Console.WriteLine("Press any key to QUIT !!!");
            Console.ReadKey();
            Console.Clear();

            #endregion Protector-typeof
        }
    }
}