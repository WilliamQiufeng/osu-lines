// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Input.Bindings;
using osu.Game.Audio;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Lines.Objects;
using osu.Game.Rulesets.Lines.Objects.Drawables;
using osu.Game.Rulesets.Lines.Objects.Drawables.Connections;
using osu.Game.Rulesets.Lines.Scores;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Lines.UI
{
    [Cached]
    public class LinesPlayfield : Playfield, IKeyBindingHandler<LinesAction>
    {
        SkinnableSound hitSample;
		private readonly PlayfieldBorder playfieldBorder;
		private readonly JudgementContainer<DrawableLinesJudgement> judgementLayer;

        private readonly IDictionary<HitResult, DrawablePool<DrawableLinesJudgement>> poolDictionary = new Dictionary<HitResult, DrawablePool<DrawableLinesJudgement>>();

        private readonly Container judgementAboveHitObjectLayer;
        private readonly FollowPointRenderer followPoints;
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);
        public readonly Bindable<Vector2> HitObjectOffsetBindable = new Bindable<Vector2>();
        public virtual Vector2 HitObjectOffset
        {
            get => HitObjectOffsetBindable.Value;
            set => HitObjectOffsetBindable.Value = value;
        }

        private readonly OrderedHitPolicy hitPolicy;

        [BackgroundDependencyLoader]
        private void load()
        {
			AddRangeInternal(new Drawable[]
			{

                hitSample = new SkinnableSound(new SampleInfo("normal-hitnormal")),
            });
		}

        public LinesPlayfield()
        {
            InternalChildren = new Drawable[]
            {
                playfieldBorder = new PlayfieldBorder { RelativeSizeAxes = Axes.Both },
                HitObjectContainer,
                followPoints = new FollowPointRenderer { RelativeSizeAxes = Axes.Both },
                judgementLayer = new JudgementContainer<DrawableLinesJudgement> { RelativeSizeAxes = Axes.Both },
                judgementAboveHitObjectLayer = new Container { RelativeSizeAxes = Axes.Both }
            };
            
            var hitWindows = new LinesHitWindows();
            foreach (var result in Enum.GetValues(typeof(HitResult)).OfType<HitResult>().Where(r => r > HitResult.None && hitWindows.IsHitResultAllowed(r)))
                poolDictionary.Add(result, new DrawableJudgementPool(result, onJudgementLoaded));

            AddRangeInternal(poolDictionary.Values);

            NewResult += onNewResult;
            hitPolicy = new OrderedHitPolicy(HitObjectContainer);
        }

        protected override void OnNewDrawableHitObject(DrawableHitObject drawableHitObject)
        {
            base.OnNewDrawableHitObject(drawableHitObject);

            DrawableLinesHitObject linesObject = (DrawableLinesHitObject)drawableHitObject;

            //maniaObject.AccentColour.Value = AccentColour;
            linesObject.CheckHittable = hitPolicy.IsHittable;
        }

        public bool OnPressed(LinesAction action)
        {
            hitSample.Play();
            return true;
        }

        public void OnReleased(LinesAction action)
        {
        }

        protected override void OnHitObjectAdded(HitObject hitObject)
        {
            base.OnHitObjectAdded(hitObject);
            followPoints.AddFollowPoints(hitObject as LinesHitObject);
            (hitObject as LinesHitObject).HitObjectOffsetBindable.BindTo(HitObjectOffsetBindable);
        }

        protected override void OnHitObjectRemoved(HitObject hitObject)
        {
            base.OnHitObjectRemoved(hitObject);
            followPoints.RemoveFollowPoints((LinesHitObject)hitObject);
        }

        private void onJudgementLoaded(DrawableLinesJudgement j)
        {
            judgementAboveHitObjectLayer.Add(j.GetProxyAboveHitObjectsContent());
        }

        private void onNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            if (!judgedObject.DisplayResult)
                return;

            DrawableLinesJudgement explosion = poolDictionary[result.Type].Get(doj => doj.Apply(result, judgedObject));
            judgementLayer.Add(explosion);
        }

        private class DrawableJudgementPool : DrawablePool<DrawableLinesJudgement>
        {
            private readonly HitResult result;
            private readonly Action<DrawableLinesJudgement> onLoaded;

            public DrawableJudgementPool(HitResult result, Action<DrawableLinesJudgement> onLoaded)
                : base(10)
            {
                this.result = result;
                this.onLoaded = onLoaded;
            }

            protected override DrawableLinesJudgement CreateNewDrawable()
            {
                var judgement = base.CreateNewDrawable();

                // just a placeholder to initialise the correct drawable hierarchy for this pool.
                judgement.Apply(new JudgementResult(new HitObject(), new Judgement()) { Type = result }, null);

                onLoaded?.Invoke(judgement);

                return judgement;
            }
        }
    }
}
