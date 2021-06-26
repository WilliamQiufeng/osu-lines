// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Lines.UI;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Lines.Objects.Drawables
{
    public class DrawableLinesHitObject : DrawableHitObject<LinesHitObject>, IKeyBindingHandler<LinesAction>
    {
        


        private readonly Circle bar;
        private readonly Container contentContainer;
        public readonly Circle TapCircle;
        private readonly EquilateralTriangle Triangle;
        private readonly Container container;
        private const float CircleDiameter = 40;
        private const float TapCircleBorderWidth = 5f;

        protected bool pressed, validPress;

        /// <summary>
        /// Whether this <see cref="DrawableManiaHitObject"/> can be hit, given a time value.
        /// If non-null, judgements will be ignored whilst the function returns false.
        /// </summary>
        public Func<DrawableHitObject, double, bool> CheckHittable;

        //protected readonly LinesAction validAction;
        public override bool HandlePositionalInput => false;
        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;
        protected int GetActionPrefix(LinesAction action)
        {
            switch (action)
            {
                case LinesAction.Left:
                    return -90;
                case LinesAction.Right:
                    return 90;
                case LinesAction.Down:
                    return 180;
                case LinesAction.Up:
                    return 0;
            }
            return 0;
        }
        public DrawableLinesHitObject(LinesHitObject hitObject)
            : base(hitObject)
        {
            Size = new Vector2(40);
            Origin = Anchor.Centre;
            Position = hitObject.Position;
            Alpha = 0.5f;

            // todo: add visuals.
            AddRangeInternal(new Drawable[]
            {
                container = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 75,
                    Height = 75,
                    Depth = 1,
                    Children = new Drawable[]
                    {

                        TapCircle = new Circle
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            //RelativeSizeAxes = Axes.Both,
                            Height = CircleDiameter + TapCircleBorderWidth * 2,
                            Width = CircleDiameter + TapCircleBorderWidth * 2,
                            Colour = Colour4.White,
                            BorderThickness = TapCircleBorderWidth,
                            BorderColour = Color4.LightBlue,
                            Masking = true,
                        },
                        bar = new Circle
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            //RelativeSizeAxes = Axes.Both,
                            Height = CircleDiameter,
                            Width = CircleDiameter,
                            Colour = Color4.White
                        },
                        Triangle = new EquilateralTriangle
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            //RelativeSizeAxes = Axes.Both,
                            Height = CircleDiameter * 0.5f,
                            Colour = Colour4.LightGreen,
                            Rotation = GetActionPrefix(hitObject.ValidAction)
                        },
                    }
                },
            });
            //validAction = hitObject.ValidAction;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            //if (Judged)
            //{
            //    pressed = false;
            //    return;
            //}

            //if (!HitObject.HitWindows.CanBeHit(timeOffset) && Time.Current > HitObject.StartTime)
            //{
            //    ApplyResult(r => r.Type = HitResult.Miss);
            //    return;
            //}

            //var result = HitObject.HitWindows.ResultFor(timeOffset);

            //if (result == HitResult.None) return;

            //if (pressed && timeOffset > (-HitObject.TimeAction))
            //{
            //    if (validPress)
            //        ApplyResult(r => r.Type = result);
            //    else
            //        ApplyResult(r => r.Type = HitResult.Miss);
            //    pressed = false;
            //}
            Debug.Assert(HitObject.HitWindows != null);

            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                    ApplyResult(r => r.Type = r.Judgement.MinResult);
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            ApplyResult(r => r.Type = result);
        }

        /// <summary>
        /// Causes this <see cref="DrawableManiaHitObject"/> to get missed, disregarding all conditions in implementations of <see cref="DrawableHitObject.CheckForResult"/>.
        /// </summary>
        public void MissForcefully() => ApplyResult(r => r.Type = r.Judgement.MinResult);

        protected override void UpdateInitialTransforms()
        {
            container.FadeInFromZero(HitObject.TimeFadeIn);
            TapCircle.ScaleTo(2.5f).ScaleTo(1, HitObject.TimePreempt, Easing.In);

            //container.ScaleTo(1, time_preempt, Easing.In);
            //this.approachPiece.MoveTo(Vector2.Zero, time_preempt, Easing.None);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            double duration = HitObject.TimeFadeIn;

            switch (state)
            {
                case ArmedState.Hit:
                    this.ScaleTo(0, duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:

                    this.FadeColour(Color4.Red, duration);
                    this.FadeOut(duration, Easing.InQuint).Expire();
                    break;
            }
        }

        public virtual bool OnPressed(LinesAction action)
        {
            //Samples.Play();

            //if (Judged)
            //    return false;

            //validPress = action == HitObject.ValidAction;
            //pressed = true;

            //return true;
            if (action != HitObject.ValidAction)
                return false;

            if (CheckHittable?.Invoke(this, Time.Current) == false)
                return false;

            return UpdateResult(true);
        }
        public virtual void OnReleased(LinesAction action)
        {
        }
    }
}
