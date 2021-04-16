using Newtonsoft.Json;
using System;

namespace MaskSensitiveData
{
    public class SimpleInfo
    {
        public int PersonalId { get; set; }
        public string PersonalName { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public string Password { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public string CreditCardNumber { get; set; }

        [JsonConverter(typeof(MaskSensitiveData))]
        public Guid PersonalIdentifier { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}