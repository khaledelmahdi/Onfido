using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Onfido.Entities
{
    public class Applicant
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("href")]
        public string HRef;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("first_name")]
        public string FirstName;

        [JsonProperty("middle_name")]
        public string MiddleName;

        [JsonProperty("last_name")]
        public string LastName;

        [JsonProperty("gender")]
        public string Gender;

        [JsonProperty("dob")]
        public DateTime DateOfBirth;

        [JsonProperty("telephone")]
        public string Telephone;

        [JsonProperty("mobile")]
        public string Mobile;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("country")]
        public string Country;

        [JsonProperty("mothers_maiden_name")]
        public string MotherMaidenName;

        [JsonProperty("previous_last_name")]
        public string PreviousLastName;

        [JsonProperty("nationality")]
        public string Nationality;

        [JsonProperty("country_of_birth")]
        public string CountryOfBirth;

        [JsonProperty("town_of_birth")]
        public string TownOfBirth;

        [JsonProperty("id_numbers")]
        public IEnumerable<IdNumber> IdNumbers;

        [JsonProperty("addresses")]
        public IEnumerable<Address> Addresses;
    }
}