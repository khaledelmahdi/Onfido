using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Onfido.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CheckType
    {
        [EnumMember(Value = "standard")]
        Standard,
        [EnumMember(Value = "express")]
        Express
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CheckStatus
    {
        [EnumMember(Value = "awaiting_data")]
        AwaitingData,
        [EnumMember(Value = "awaiting_approval")]
        AwaitingApproval
        ,
        [EnumMember(Value = "complete")]
        Complete
        ,
        [EnumMember(Value = "withdrawn")]
        Withdrawn
        ,
        [EnumMember(Value = "paused")]
        Paused
        ,
        [EnumMember(Value = "cancelled")]
        Cancelled
    }

    public class Check
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("href")]
        public string HRef;

        [JsonProperty("type")]
        public CheckType Type;

        [JsonProperty("status")]
        public CheckStatus Status;

        [JsonProperty("result")]
        public string Result;

        [JsonProperty("download_uri")]
        public string DownloadUri;

        [JsonProperty("form_uri")]
        public string FormUri;

        [JsonProperty("redirect_uri")]
        public string RedirectUri;

        [JsonProperty("results_uri")]
        public string ResultsUri;

        [JsonProperty("reports")]
        public IEnumerable<Report> Reports;
    }
}