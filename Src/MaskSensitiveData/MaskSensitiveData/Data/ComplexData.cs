using System;
using System.Collections.Generic;

namespace MaskSensitiveData.CustomSerializer
{
    public class ComplexData
    {
        public int Id { get; set; }

        [SensitiveData]
        public string FirstName { get; set; }

        [SensitiveData]
        public string LastName { get; set; }

        [SensitiveData("YYYY-MM-DD")]
        public DateTime DateOfBirth { get; set; }

        [SensitiveData("99999999-9999-9999-9999-999999999999")]
        public Guid PersonIdentifier { get; set; }

        public bool IsActive { get; set; }
        public Position CurrentPosition { get; set; }
        public IList<Job> Jobs { get; set; }
    }

    public class Job
    {
        public int Id { get; set; }

        [SensitiveData("AAAAAAAAAA")]
        public string Company { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public IList<Position> Positions { get; set; }
    }

    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [SensitiveData("##########")]
        public Decimal Salary { get; set; }

        public bool IsCurrent { get; set; }
    }
}