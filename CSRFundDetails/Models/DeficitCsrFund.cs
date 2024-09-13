using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSRFundDetails.Models
{
	public class DeficitCsrFund
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }  // MongoDB's default _id field
        [Required]
        public string OrganizationId { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        public string ProjectName { get; set; }

        public string ProjectReceivedFrom { get; set; }

        public string ImpactOutcome { get; set; }

        public string TargetBeneficiaries { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ProjectValue { get; set; }
    }
}

