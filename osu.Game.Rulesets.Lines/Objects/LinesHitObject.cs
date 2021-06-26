// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Lines.Objects
{
    public class LinesHitObject : HitObject, IHasPosition
    {
        public double TimePreempt = 850;
        public double TimeFadeIn = 300;
        public double TimeAction = 150;

        /// <summary>
        /// Minimum preempt time at AR=10.
        /// </summary>
        public const double PREEMPT_MIN = 450;
        public const float OBJECT_RADIUS = 60;

        public override Judgement CreateJudgement() => new Judgement();
        public readonly Bindable<Vector2> PositionBindable = new Bindable<Vector2>();
        public virtual Vector2 Position
        {
            get => PositionBindable.Value;
            set => PositionBindable.Value = value;
        }

        public Vector2 StackedPosition => Position + StackOffset;

        public virtual Vector2 EndPosition => Position;

        public Vector2 StackedEndPosition => EndPosition + StackOffset;

        public readonly Bindable<int> StackHeightBindable = new Bindable<int>();

        public int StackHeight
        {
            get => StackHeightBindable.Value;
            set => StackHeightBindable.Value = value;
        }

        public Vector2 StackOffset => new Vector2(StackHeight * Scale * -6.4f);

        public double Radius => OBJECT_RADIUS * Scale;

        public readonly Bindable<float> ScaleBindable = new BindableFloat(1);

        public float Scale
        {
            get => ScaleBindable.Value;
            set => ScaleBindable.Value = value;
        }

        public float X => Position.X;
        public float Y => Position.Y;
        public LinesAction ValidAction;
        public Vector2 PreviousObjectPosition;

        public LinesHitObject()
        {
            StackHeightBindable.BindValueChanged(height =>
			{
				foreach (var nested in NestedHitObjects.OfType<LinesHitObject>())
					nested.StackHeight = height.NewValue;
			});
		}

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)BeatmapDifficulty.DifficultyRange(difficulty.ApproachRate, 1800, 1200, PREEMPT_MIN);

            // Preempt time can go below 450ms. Normally, this is achieved via the DT mod which uniformly speeds up all animations game wide regardless of AR.
            // This uniform speedup is hard to match 1:1, however we can at least make AR>10 (via mods) feel good by extending the upper linear function above.
            // Note that this doesn't exactly match the AR>10 visuals as they're classically known, but it feels good.
            // This adjustment is necessary for AR>10, otherwise TimePreempt can become smaller leading to hitcircles not fully fading in.
            TimeFadeIn = 400 * Math.Min(1, TimePreempt / PREEMPT_MIN);

            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }

    }
}
