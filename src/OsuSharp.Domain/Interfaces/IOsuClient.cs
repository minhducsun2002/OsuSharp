﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using OsuSharp.Builders;
using OsuSharp.Domain;

namespace OsuSharp.Interfaces
{
    /// <summary>
    /// Interfaces the osu client for communicating with the osu! api.
    /// </summary>
    public interface IOsuClient : IDisposable
    {
        /// <summary>
        /// Gets the configuration of the client.
        /// </summary>
        IOsuClientConfiguration Configuration { get; }

        /// <summary>
        /// Gets or requests an API access token. This method will use Client Credential Grant unless
        /// A refresh token is present on the current <see cref="IOsuToken" /> instance.
        /// </summary>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IOsuToken" />.
        /// </returns>
        ValueTask<IOsuToken> GetOrUpdateAccessTokenAsync(
            CancellationToken token = default);

        /// <summary>
        /// Updates the current osu! api credentials by the given access, refresh tokens and the expiry time.
        /// </summary>
        /// <param name="accessToken">
        /// Access token.
        /// </param>
        /// <param name="refreshToken">
        /// Refresh token.
        /// </param>
        /// <param name="expiresIn">
        /// Amount of seconds before the token expires.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IOsuToken" />.
        /// </returns>
        /// <remarks>
        /// If you are going to use the authorization code grant, use this method to create your <see cref="IOsuToken" />.
        /// </remarks>
        IOsuToken UpdateAccessToken(
            [NotNull] string accessToken,
            [NotNull] string refreshToken,
            long expiresIn);

        /// <summary>
        /// Revokes the current access token.
        /// </summary>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        Task RevokeAccessTokenAsync(
            CancellationToken token = default);

        /// <summary>
        /// Gets a user's kudosu history from the API.
        /// </summary>
        /// <param name="userId">
        /// Id of the user.
        /// </param>
        /// <param name="limit">
        /// Limit number of results.
        /// </param>
        /// <param name="offset">
        /// Offset of result for pagination.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a set of <see cref="IKudosuHistory" />.
        /// </returns>
        Task<IReadOnlyList<IKudosuHistory>> GetUserKudosuAsync(
            long userId,
            int? limit = null,
            int? offset = null,
            CancellationToken token = default);

        /// <summary>
        /// Gets a user from the API.
        /// </summary>
        /// <param name="username">
        /// Username of the user.
        /// </param>
        /// <param name="gameMode">
        /// Gamemode of the user. Defaults gamemode is picked when null.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IUser" />.
        /// </returns>
        Task<IUser> GetUserAsync(
            [NotNull] string username,
            GameMode? gameMode = null,
            CancellationToken token = default);

        /// <summary>
        /// Gets a user from the API.
        /// </summary>
        /// <param name="userId">
        /// Id of the user.
        /// </param>
        /// <param name="gameMode">
        /// Gamemode of the user. Defaults gamemode is picked when null.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IUser" />.
        /// </returns>
        Task<IUser> GetUserAsync(
            long userId,
            GameMode? gameMode = null, 
            CancellationToken token = default);

        /// <summary>
        /// Gets a user's recent activity history from the API.
        /// </summary>
        /// <param name="userId">
        /// Id of the user.
        /// </param>
        /// <param name="limit">
        /// Limit number of results.
        /// </param>
        /// <param name="offset">
        /// Offset of result for pagination.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a set of <see cref="IEvent" />s.
        /// </returns>
        Task<IReadOnlyList<IEvent>> GetUserRecentAsync(
            long userId,
            int? limit = null,
            int? offset = null, 
            CancellationToken token = default);

        /// <summary>
        /// Gets a user's beatmapsets from the API.
        /// </summary>
        /// <param name="userId">
        /// Id of the user.
        /// </param>
        /// <param name="type">
        /// Type of the beatmapsets to look-up.
        /// </param>
        /// <param name="limit">
        /// Limit number of results.
        /// </param>
        /// <param name="offset">
        /// Offset of result for pagination.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a set of <see cref="IBeatmapset" />s.
        /// </returns>
        Task<IReadOnlyList<IBeatmapset>> GetUserBeatmapsetsAsync(
            long userId,
            BeatmapsetType type,
            int? limit = null,
            int? offset = null,
            CancellationToken token = default);

        /// <summary>
        /// Gets a user's scores from the API.
        /// </summary>
        /// <param name="userId">
        /// Id of the user.
        /// </param>
        /// <param name="type">
        /// Type of the scores to look-up.
        /// </param>
        /// <param name="includeFails">
        /// Whether to include failed scores.
        /// </param>
        /// <param name="gameMode">
        /// Game mode to lookup scores for.
        /// </param>
        /// <param name="limit">
        /// Limit number of results.
        /// </param>
        /// <param name="offset">
        /// Offset of result for pagination.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a set of <see cref="IScore" />s.
        /// </returns>
        Task<IReadOnlyList<IScore>> GetUserScoresAsync(
            long userId,
            ScoreType type,
            bool includeFails = false,
            GameMode? gameMode = null,
            int? limit = null,
            int? offset = null,
            CancellationToken token = default);

        /// <summary>
        /// Gets the current authenticated user from the API.
        /// </summary>
        /// <param name="gameMode">
        /// Gamemode of the user. Defaults gamemode is picked when null.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IUser" />.
        /// </returns>
        Task<IUser> GetCurrentUserAsync(
            GameMode? gameMode = null,
            CancellationToken token = default);

        /// <summary>
        /// Gets a beatmap by its id from the API.
        /// </summary>
        /// <param name="beatmapId">
        /// Id of the beatmap.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IBeatmap" />.
        /// </returns>
        Task<IBeatmap> GetBeatmapAsync(
            long beatmapId,
            CancellationToken token = default);

        /// <summary>
        /// Gets a beatmapset by its id from the API.
        /// </summary>
        /// <param name="beatmapsetId">
        /// Id of the beatmap.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IBeatmapset" />.
        /// </returns>
        Task<IBeatmapset> GetBeatmapsetAsync(
            long beatmapsetId,
            CancellationToken token = default);

        /// <summary>
        /// Gets a user score on a beatmap from the API.
        /// </summary>
        /// <param name="beatmapId">
        /// If of the beatmap.
        /// </param>
        /// <param name="userId">
        /// Id of the user.
        /// </param>
        /// <param name="gameMode">
        /// Gamemode of the user. Defaults gamemode is picked when null.
        /// </param>
        /// <param name="mods">
        /// Mods to filter when looking for a score.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IBeatmapUserScore" />.
        /// </returns>
        Task<IBeatmapUserScore> GetUserBeatmapScoreAsync(
            long beatmapId,
            long userId,
            GameMode? gameMode = null,
            Mods? mods = null,
            CancellationToken token = default);

        /// <summary>
        /// Gets the top scores on a beatmap from the API.
        /// </summary>
        /// <param name="beatmapId">
        /// If of the beatmap.
        /// </param>
        /// <param name="gameMode">
        /// Gamemode of the user. Defaults gamemode is picked when null.
        /// </param>
        /// <param name="mods">
        /// Mods to filter when looking for a score.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IBeatmapScores" />.
        /// </returns>
        Task<IBeatmapScores> GetBeatmapScoresAsync(
            long beatmapId,
            GameMode? gameMode = null,
            Mods? mods = null,
            CancellationToken token = default);

        /// <summary>
        /// Lookups a beatmap by either its id, checksum or filename.
        /// </summary>
        /// <param name="id">
        /// Id of the beatmap.
        /// </param>
        /// <param name="checksum">
        /// Checksum of the beatmap.
        /// </param>
        /// <param name="filename">
        /// Filename of the beatmap.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IBeatmap"/>
        /// </returns>
        Task<IBeatmap> LookupBeatmapAsync(
            long? id = null,
            string checksum = null,
            string filename = null,
            CancellationToken token = default);

        /// <summary>
        /// Enumerates the available beatmapsets from the API.
        /// </summary>
        /// <param name="builder">
        /// Builder that filters the search.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns an asynchronous enumeration of <see cref="IBeatmapset"/>
        /// </returns>
        IAsyncEnumerable<IBeatmapset> EnumerateBeatmapsetsAsync(
            BeatmapsetsLookupBuilder builder = null,
            BeatmapSorting sorting = BeatmapSorting.Ranked_Desc,
            CancellationToken token = default);

        /// <summary>
        /// Gets the current seasonal backgrounds from the API.
        /// </summary>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="ISeasonalBackgrounds"/>
        /// </returns>
        Task<ISeasonalBackgrounds> GetSeasonalBackgroundsAsync(
            CancellationToken token = default);

        /// <summary>
        /// Gets a score by its ID from the API.
        /// </summary>
        /// <param name="scoreId">
        /// Id of the score
        /// </param>
        /// <param name="gameMode">
        /// Game mode the score was playing in.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IScore"/>
        /// </returns>
        Task<IScore> GetScoreAsync(
            long scoreId,
            GameMode gameMode = GameMode.Osu,
            CancellationToken token = default);

        /// <summary>
        /// Gets a replay by its score ID from the API.
        /// </summary>
        /// <param name="scoreId">
        /// Id of the score
        /// </param>
        /// <param name="gameMode">
        /// Game mode the score was playing in.
        /// </param>
        /// <param name="token">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Returns a <see cref="IScore"/>
        /// </returns>
        Task<IReplay> GetReplayAsync(
            long scoreId,
            GameMode gameMode = GameMode.Osu,
            CancellationToken token = default);
    }
}