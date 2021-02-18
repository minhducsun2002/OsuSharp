﻿using Newtonsoft.Json;

namespace OsuSharp.Entities
{
    public sealed class EventBeatmapsetModel
    {
        [JsonProperty("title")]
        public string Title { get; internal set; }
        
        [JsonProperty("url")]
        public string Url { get; internal set; }
        
        internal EventBeatmapsetModel()
        {
            
        }
    }
}