using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Configuration;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Lines.Objects.Drawables
{
    public class DrawableLinesJudgement : DrawableJudgement
    {
        //protected Lighting Lighting { get; private set; }

        [Resolved]
        private OsuConfigManager config { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            //AddInternal(Lighting = new SkinnableLighting
            //{
            //    Anchor = Anchor.Centre,
            //    Origin = Anchor.Centre,
            //    Blending = BlendingParameters.Additive,
            //    Depth = float.MaxValue,
            //    Alpha = 0
            //});
        }

        protected override void PrepareForUse()
        {
            base.PrepareForUse();

            //Lighting.ResetAnimation();
            //Lighting.SetColourFrom(JudgedObject, Result);

            //if (JudgedObject?.HitObject is LinesHitObject linesObject)
            //{
            //    Position = linesObject.Position;
            //    Scale = new Vector2(1);
            //}
        }

        protected override void ApplyHitAnimations()
        {
            bool hitLightingEnabled = config.Get<bool>(OsuSetting.HitLighting);

            //Lighting.Alpha = 0;

            //if (hitLightingEnabled && Lighting.Drawable != null)
            //{
            //    // todo: this animation changes slightly based on new/old legacy skin versions.
            //    Lighting.ScaleTo(0.8f).ScaleTo(1.2f, 600, Easing.Out);
            //    Lighting.FadeIn(200).Then().Delay(200).FadeOut(1000);

            //    // extend the lifetime to cover lighting fade
            //    LifetimeEnd = Lighting.LatestTransformEndTime;
            //}

            base.ApplyHitAnimations();
        }

        protected override Drawable CreateDefaultJudgement(HitResult result) => new LinesJudgementPiece(result);

        private class LinesJudgementPiece : DefaultJudgementPiece
        {
            public LinesJudgementPiece(HitResult result)
                : base(result)
            {
            }

            public override void PlayAnimation()
            {
                base.PlayAnimation();

                if (Result != HitResult.Miss)
                    JudgementText.TransformSpacingTo(Vector2.Zero).Then().TransformSpacingTo(new Vector2(14, 0), 1800, Easing.OutQuint);
            }
        }
    }
}
