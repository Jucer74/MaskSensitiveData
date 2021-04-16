using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaskSensitiveData
{
    public class SimpleData
    {
        public int PersonalId { get; set; }
        public string PersonalName { get; set; }

        [SensitiveData]
        public string Password { get; set; }

        [SensitiveData("##########")]
        public string CreditCardNumber { get; set; }

        [SensitiveData("99999999-9999-9999-9999-999999999999")]
        public Guid PersonalIdentifier { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}