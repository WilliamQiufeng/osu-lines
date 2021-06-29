// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Lines
{
    public class LinesDifficultyCalculator : DifficultyCalculator
    {
        public LinesDifficultyCalculator(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, double clockRate)
        {
            var AverageTimeDifference = GetAverageTimeDifference(beatmap);
            return new DifficultyAttributes(mods, skills, AllowedMaxDifference / AverageTimeDifference);
        }

        protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, double clockRate) => Enumerable.Empty<DifficultyHitObject>();

        protected override Skill[] CreateSkills(IBeatmap beatmap, Mod[] mods, double clockRate) => new Skill[0];


        public static readonly float AllowedMaxDifference = 1500;
        public static float GetAverageTimeDifference(IBeatmap beatmap)
        {
            var count = beatmap.HitObjects.Count;
            if (count == 0) return AllowedMaxDifference;
            double sum = 0;
            int avg_count = 0;
            double MaxDifference = 0;
            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                {
                    var diff = beatmap.HitObjects[i].StartTime - beatmap.HitObjects[i - 1].StartTime;
                    if (diff > AllowedMaxDifference) continue;
                    sum += diff;
                    if (diff != 0) avg_count++;
                    MaxDifference = Math.Max(MaxDifference, diff);
                }
            }
            float AverageTimeDifference = (float)sum / avg_count;
            return AverageTimeDifference;
        }
    }
}
