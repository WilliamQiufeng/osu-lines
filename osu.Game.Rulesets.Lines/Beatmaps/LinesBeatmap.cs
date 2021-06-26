using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Lines.Objects;

namespace osu.Game.Rulesets.Lines.Beatmaps
{
    public class LinesBeatmap : Beatmap<LinesHitObject>
    {

        /// <summary>
        /// Creates a new <see cref="ManiaBeatmap"/>.
        /// </summary>
        public LinesBeatmap()
        {
        }

        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int notes = HitObjects.Count(s => s is LinesHitObject);
            //int holdnotes = HitObjects.Count(s => s is HoldNote);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Note Count",
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles),
                    Content = notes.ToString(),
                },
                //new BeatmapStatistic
                //{
                //    Name = @"Hold Note Count",
                //    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Sliders),
                //    Content = holdnotes.ToString(),
                //},
            };
        }

    }
}
