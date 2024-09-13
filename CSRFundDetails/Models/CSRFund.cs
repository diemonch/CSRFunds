using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSRFundDetails.Models
{
    public class CsrFund
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }  // MongoDB's default _id field
        [Required]
        public string OrganizationId { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OpeningBalance { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CsrFundAlloted { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ProjectedExpenditure { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CsrFundBalance { get; set; }
    }
}

