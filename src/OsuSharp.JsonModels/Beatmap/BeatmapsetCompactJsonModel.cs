﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace OsuSharp.JsonModels
{
    public class BeatmapsetCompactJsonModel : JsonModel
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("artist_unicode")]
        public string ArtistUnicode { get; set; }

        [JsonProperty("covers")]
        public BeatmapCoverJsonModel CoversJsonModel { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("favorite_count")]
        public int FavoriteCount { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("play_count")]
        public int PlayCount { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_unicode")]
        public string TitleUnicode { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("video")]
        public bool HasVideo { get; set; }

        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }

        [JsonProperty("beatmaps")]
        public IReadOnlyList<BeatmapJsonModel> Beatmaps { get; set; }

        // todo: type
        [JsonProperty("converts")]
        public object Converts { get; set; }

        // todo: type
        [JsonProperty("current_user_attributes")]
        public object CurrentUserAttributes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        // todo: type
        [JsonProperty("discussions")]
        public object Discussions { get; set; }

        // todo: type
        [JsonProperty("events")]
        public object Events { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("has_favourited")]
        public bool? HasFavourited { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        // todo: type
        [JsonProperty("nominations")]
        public object Nominations { get; set; }

        // todo: type
        [JsonProperty("rating")]
        public object Rating { get; set; }

        // todo: type
        [JsonProperty("recent_favourites")]
        public object RecentFavourites { get; set; }

        // todo: type
        [JsonProperty("related_users")]
        public object RelatedUsers { get; set; }

        // todo: type
        [JsonProperty("user")]
        public object User { get; set; }
    }
}