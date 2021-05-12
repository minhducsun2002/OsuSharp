﻿using System;
using Newtonsoft.Json;

namespace OsuSharp.JsonModels
{
    public sealed class UserBadgeJsonModel
    {
        [JsonProperty("awarded_at")]
        public DateTimeOffset AwardedAt { get; internal set; }
        
        [JsonProperty("description")]
        public string Description { get; internal set; }
        
        [JsonProperty("image_url")]
        public string ImageUrl { get; internal set; }
        
        [JsonProperty("url")]
        public string Url { get; internal set; }
        
        internal UserBadgeJsonModel()
        {
            
        }
    }
}