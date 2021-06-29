// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Lines.Objects;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using System;
using osuTK;
using osu.Game.Rulesets.Lines.UI;

namespace osu.Game.Rulesets.Lines.Beatmaps
{
    public class LinesBeatmapConverter : BeatmapConverter<LinesHitObject>
    {
        private LinesHitObject Prev;
        private Vector2 SumUpPos = new Vector2(0, 0);
        // 120 pix : 1000 ms
        private float DistanceTimeRatio = 0.12f;
        private float AverageTimeDifference = 0;
        private Vector2 PreviousObjectPosition;
        public LinesBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
            AverageTimeDifference = LinesDifficultyCalculator.GetAverageTimeDifference(beatmap);
            DistanceTimeRatio = 100 / AverageTimeDifference;
            Console.WriteLine($"Converting {beatmap.Metadata.Title} {ruleset.ShortName}");
            Console.WriteLine($"DistanceTimeRatio: {DistanceTimeRatio}");
            Console.WriteLine($"AverageTimeDifference: {AverageTimeDifference}");
        }

        // todo: Check for conversion types that should be supported (ie. Beatmap.HitObjects.Any(h => h is IHasXPosition))
        // https://github.com/ppy/osu/tree/master/osu.Game/Rulesets/Objects/Types
        public override bool CanConvert() => true;

        
        protected override IEnumerable<LinesHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            var rand = new Random();
            var ObjectPos = LinesPlayfield.BASE_SIZE / 2;
            var validAction = GetAction(original is IHasColumn col ? col.Column : rand.Next(0, 4));
            var vec = GetObjectOffset(validAction);
            if (Prev != null)
            {
                ObjectPos = GetExpectedPosition(original.StartTime);
                if (Prev.StartTime == original.StartTime)
                {
                    SumUpPos += vec;
                }
                else
                {
                    SumUpPos = vec;
                    PreviousObjectPosition = ObjectPos;
                }
            }
            else
            {
                SumUpPos = vec;
                PreviousObjectPosition = ObjectPos;
            }
            // Prevent HitObjects placed outside playfield
            //if (ObjectPos.X < 0)
            //    ObjectPos.X += LinesPlayfield.BASE_SIZE.X;
            //else if (ObjectPos.X > LinesPlayfield.BASE_SIZE.X)
            //    ObjectPos.X -= LinesPlayfield.BASE_SIZE.X;
            //if (ObjectPos.Y < 0)
            //    ObjectPos.Y += LinesPlayfield.BASE_SIZE.Y;
            //else if (ObjectPos.Y > LinesPlayfield.BASE_SIZE.Y)
            //    ObjectPos.Y -= LinesPlayfield.BASE_SIZE.Y;
            yield return Prev = new LinesHitObject
            {
                Samples = original.Samples,
                StartTime = original.StartTime,
                ValidAction = validAction,
                Position = ObjectPos,
                PreviousObject = Prev,
            };
        }

		protected override Beatmap<LinesHitObject> CreateBeatmap()
		{
			return new LinesBeatmap();
		}

        protected Vector2 GetExpectedPosition(double start_time)
		{
            var ObjectPos = Prev.Position;
            float difference = (float)(start_time - Prev.StartTime);
            difference = Math.Clamp(difference, 0, LinesDifficultyCalculator.AllowedMaxDifference);
            return ObjectPos + SumUpPos * DistanceTimeRatio * difference;
        }

		protected LinesAction GetAction(int X)
		{
            switch (X % 4)
			{
                case 0:
                    return LinesAction.Left;
                case 1:
                    return LinesAction.Down;
                case 2:
                    return LinesAction.Up;
                case 3:
                    return LinesAction.Right;
			}
            throw new InvalidOperationException("Unknown column");
		}

        protected Vector2 GetObjectOffset(LinesAction action)
		{
            switch (action)
			{
                case LinesAction.Left:
                    return new Vector2(-1, 0);
                case LinesAction.Down:
                    return new Vector2(0, 1);
                case LinesAction.Up:
                    return new Vector2(0, -1);
                case LinesAction.Right:
                    return new Vector2(1, 0);
			}
            throw new InvalidOperationException("Unknown LinesAction type");
		}
    }
}
