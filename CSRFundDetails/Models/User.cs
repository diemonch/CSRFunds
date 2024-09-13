using System;
using MongoDB.Bson;

namespace CSRFundDetails.Models
{
   
        public class User
        {
        public ObjectId Id { get; set; } // MongoDB ObjectId
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string OrganizationId { get; set; }  // Maps user to an organization
    }
    
}

