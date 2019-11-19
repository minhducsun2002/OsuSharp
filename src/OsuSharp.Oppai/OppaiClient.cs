﻿#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace OsuSharp.Oppai
{
    public static class OppaiClient
    {
        /// <summary>
        ///     Creates an instance of oppai.
        /// </summary>
        [DllImport(@"oppai.dll")]
        private static extern IntPtr ezpp_new();

        /// <summary>
        ///     Frees the instance of ezpp.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern void ezpp_free(IntPtr handle);

        /// <summary>
        ///     Sets the current map.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        /// <param name="data">Byte array of beatmap .osu file.</param>
        /// <param name="data_size">Size of the byte array.</param>
        [DllImport(@"oppai.dll")]
        private static extern int ezpp_data(IntPtr handle, byte[] data, int data_size);

        /// <summary>
        ///     Sets the accuracy.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        /// <param name="accuracy_percent">Amount of accuracy. Must be between 0 and 1.</param>
        [DllImport(@"oppai.dll")]
        private static extern void ezpp_set_accuracy_percent(IntPtr handle, float accuracy_percent);

        /// <summary>
        ///     Sets the mods flags.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        /// <param name="mods">Flag representing every mods set.</param>
        [DllImport(@"oppai.dll")]
        private static extern void ezpp_set_mods(IntPtr handle, int mods);

        /// <summary>
        ///     Toggles autocalc so it calculates (or doesn't) automatically new amount of pp after changing accuracy or mods.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        /// <param name="autocalc">0 to disable autocalc, 1 to enable autocalc</param>
        [DllImport(@"oppai.dll")]
        private static extern void ezpp_set_autocalc(IntPtr handle, int autocalc);

        /// <summary>
        ///     Returns the amount of pp with the previously set parameters.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern float ezpp_pp(IntPtr handle);

        /// <summary>
        ///     Returns the AR of the current beatmap.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern float ezpp_ar(IntPtr handle);

        /// <summary>
        ///     Returns the OD of the current beatmap.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern float ezpp_od(IntPtr handle);

        /// <summary>
        ///     Returns the CS of the current beatmap.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern float ezpp_cs(IntPtr handle);

        /// <summary>
        ///     Returns the HP of the current beatmap.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern float ezpp_hp(IntPtr handle);

        /// <summary>
        ///     Returns the stars of the current beatmap.
        /// </summary>
        /// <param name="handle">Ptr of ezpp.</param>
        [DllImport(@"oppai.dll")]
        private static extern float ezpp_stars(IntPtr handle);

        /// <summary>
        ///     Returns the version of oppai.
        /// </summary>
        /// <returns></returns>
        [DllImport(@"oppai.dll", CharSet = CharSet.Unicode)]
        private static extern string oppai_version_str();

        /// <summary>
        ///     Instance of the HttpClient to download the beatmap data.
        /// </summary>
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        ///     Semaphore to avoid concurrency issues as we depend on what is set on oppai.dll.
        /// </summary>
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        static OppaiClient()
        {
            if (!File.Exists("./oppai.dll"))
            {
                throw new NotSupportedException(
                    "oppai.dll has not been found and assembly OsuSharp.Oppai cannot work without it. " +
                    "This dll must be next to your main assembly dll/exe.");
            }
        }

        /// <summary>
        ///     Gets the beatmap data by its id.
        /// </summary>
        /// <param name="beatmapId">Id of the beatmap.</param>
        private static Task<byte[]> GetBeatmapDataAsync(long beatmapId)
        {
            return _httpClient.GetByteArrayAsync($"https://osu.ppy.sh/osu/{beatmapId}");
        }

        /// <summary>
        ///     Gets the current oppai's version.
        /// </summary>
        public static string GetOppaiVersion()
        {
            return oppai_version_str();
        }

        /// <summary>
        ///     Gets the amount of pp for the map. No mod and accuracy of 100%.
        /// </summary>
        /// <param name="beatmapId">Id of the beatmap.</param>
        public static Task<PerformanceData> GetPPAsync(long beatmapId)
        {
            return GetPPAsync(beatmapId, Mode.None, 100.0F);
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified accuracy.
        /// </summary>
        /// <param name="beatmapId">If of the beatmap.</param>
        /// <param name="accuracy">Accuracy percents to calculate pps for.</param>
        public static async Task<PerformanceData> GetPPAsync(long beatmapId, float accuracy)
        {
            var pps = await GetPPAsync(beatmapId, new float[] { accuracy });
            return pps[accuracy];
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified mods.
        /// </summary>
        /// <param name="beatmapId">If of the beatmap.</param>
        /// <param name="accuracy">Accuracy percents to calculate pps for.</param>
        public static async Task<PerformanceData> GetPPAsync(long beatmapId, Mode mode)
        {
            var pps = await GetPPAsync(beatmapId, new Mode[] { mode });
            return pps[mode];
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified mods and accuracy.
        /// </summary>
        /// <param name="beatmapId"></param>
        /// <param name="mode"></param>
        /// <param name="accuracy"></param>
        /// <returns></returns>
        public static async Task<PerformanceData> GetPPAsync(long beatmapId, Mode mode, float accuracy)
        {
            var pps = await GetPPAsync(beatmapId, new Mode[] { mode }, new float[] { accuracy });
            return pps[mode][accuracy];
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified accuracies.
        /// </summary>
        /// <param name="beatmapId">If of the beatmap.</param>
        /// <param name="accuracies">Accuracies percents to calculate pps for.</param>
        public static Task<ReadOnlyDictionary<float, PerformanceData>> GetPPAsync(long beatmapId, float[] accuracies)
        {
            return GetPPAsync(beatmapId, 0, accuracies);
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified mods.
        /// </summary>
        /// <param name="beatmapId"></param>
        /// <param name="mods"></param>
        /// <returns></returns>
        public static Task<ReadOnlyDictionary<Mode, PerformanceData>> GetPPAsync(long beatmapId, Mode[] mods)
        {
            return GetPPAsync(beatmapId, mods, 100.0F);
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified mod and accuracies.
        /// </summary>
        /// <param name="beatmapId">Id of the beatmap.</param>
        /// <param name="mods">Mods to calculate pps for.</param>
        /// <param name="accuracies">Accuracies to calculate pps for.</param>
        public static async Task<ReadOnlyDictionary<float, PerformanceData>> GetPPAsync(long beatmapId, Mode mods,
            float[] accuracies)
        {
            var pps = await GetPPAsync(beatmapId, new Mode[] { mods }, accuracies);
            return pps[mods];
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified mods and accuracy.
        /// </summary>
        /// <param name="beatmapId"></param>
        /// <param name="accuracy"></param>
        /// <param name="mods"></param>
        /// <returns></returns>
        public static async Task<ReadOnlyDictionary<Mode, PerformanceData>> GetPPAsync(long beatmapId, Mode[] mods, float accuracy)
        {
            var pps = await GetPPAsync(beatmapId, mods, new float[] { accuracy });
            return new ReadOnlyDictionary<Mode, PerformanceData>(pps.ToDictionary(x => x.Key, y => y.Value.First().Value));
        }

        /// <summary>
        ///     Gets the amount of pp for the map with the specified mods and accuracies.
        /// </summary>
        /// <param name="beatmapId">Id of the beatmap.</param>
        /// <param name="mods">Mods to calculate pps for.</param>
        /// <param name="accuracies">Accuracies to calculate pps for.</param>
        public static async Task<ReadOnlyDictionary<Mode, ReadOnlyDictionary<float, PerformanceData>>> GetPPAsync(
            long beatmapId, Mode[] mods, float[] accuracies)
        {
            await _semaphore.WaitAsync();

            var modsAccuracies = new Dictionary<Mode, ReadOnlyDictionary<float, PerformanceData>>();
            var readonlyModsAccuracies = new ReadOnlyDictionary<Mode, ReadOnlyDictionary<float, PerformanceData>>(modsAccuracies);

            try
            {
                var ptr = ezpp_new();
                ezpp_set_autocalc(ptr, 1);

                var beatmap = await GetBeatmapDataAsync(beatmapId);

                ezpp_data(ptr, beatmap, beatmap.Length);
                for (var j = 0; j < mods.Length; ++j)
                {
                    ezpp_set_mods(ptr, (int)mods[j]);

                    var pps = new Dictionary<float, PerformanceData>();
                    var dict = new ReadOnlyDictionary<float, PerformanceData>(pps);

                    for (var i = 0; i < accuracies.Length; ++i)
                    {
                        var accuracy = accuracies[i];
                        if (accuracy <= 1)
                        {
                            accuracy *= 100.0F;
                        }

                        ezpp_set_accuracy_percent(ptr, accuracy);

                        pps[accuracies[i]] = new PerformanceData(ezpp_pp(ptr), ezpp_stars(ptr), 
                            ezpp_ar(ptr), ezpp_od(ptr), ezpp_cs(ptr), ezpp_hp(ptr), accuracy, mods[j]);
                    }

                    modsAccuracies[mods[j]] = dict;
                }

                ezpp_free(ptr);
            }
            finally
            {
                _semaphore.Release();
            }

            return readonlyModsAccuracies;
        }
    }
}
