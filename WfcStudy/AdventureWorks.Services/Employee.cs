using System;
using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

namespace AdventureWorks.Services
{
    [DataContract]
    public class Employee
    {
        [DataMember]
        public virtual int BusinessEntityID { get; set; }
        [DataMember]
        public virtual string NationalIDNumber { get; set; }
        [DataMember]
        public virtual string LoginID { get; set; }
        [DataMember]
        public virtual string OrganizationNode { get; set; }
        [DataMember]
        public virtual short? OrganizationLevel { get; set; }
        [DataMember]
        public virtual string JobTitle { get; set; }
        [DataMember]
        public virtual DateTime BirthDate { get; set; }
        [DataMember]
        public virtual string MaritalStatus { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual DateTime HireDate { get; set; }
        [DataMember]
        public virtual bool SalariedFlag { get; set; }
        [DataMember]
        public virtual short VacationHours { get; set; }
        [DataMember]
        public virtual short SickLeaveHours { get; set; }
        [DataMember]
        public virtual bool CurrentFlag { get; set; }
        [DataMember]
        public virtual Guid rowguid { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
    }
}
