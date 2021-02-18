﻿using Newtonsoft.Json;

namespace OsuSharp.Entities
{
    public sealed class BeatmapPlaycountEvent : Event
    {
        [JsonProperty("count")]
        public int Count { get; internal set; }
        
        [JsonProperty("beatmap")]
        public EventBeatmapModel Beatmap { get; internal set; }

        internal BeatmapPlaycountEvent()
        {
            
        }
    }
}