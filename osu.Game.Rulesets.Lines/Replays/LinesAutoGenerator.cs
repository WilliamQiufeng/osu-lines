// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Lines.Objects;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Lines.Replays
{
    public class LinesAutoGenerator : AutoGenerator<LinesReplayFrame>
    {
        public new Beatmap<LinesHitObject> Beatmap => (Beatmap<LinesHitObject>)base.Beatmap;

        public LinesAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new LinesReplayFrame());
            //double prevTime = 0;
            for (int i = 0; i < Beatmap.HitObjects.Count; i++)
            {
                var hitObject = Beatmap.HitObjects[i];
                var hitTime = hitObject.StartTime/* + hitObject.HitWindows.WindowFor(HitResult.Perfect)*/;
                //var lerp = prevTime + (hitTime - prevTime) * 0.1;
                Frames.Add(new LinesReplayFrame(hitTime, hitObject.ValidAction));
                Frames.Add(new LinesReplayFrame(hitTime + 0.1));
            }
        }
    }
}
