using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MaskSensitiveData
{
    public enum PaymentType
    {
        Cash,
        CreditCard
    }

    public class ComplexInfo
    {
        public int CustomerId { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public Guid CustomerIdentifier { get; set; }

        public string CustomerName { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public string Password { get; set; }

        public IList<PaymentMethod> Payments { get; set; }
    }

    public class PaymentMethod
    {
        [JsonConverter(typeof(MaskSensitiveData))]
        public DateTime CreditCardExpirationDate { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public string CreditCardNumber { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public PaymentType PaymentType { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}