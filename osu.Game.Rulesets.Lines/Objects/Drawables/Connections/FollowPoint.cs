using System;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Lines.Objects.Drawables.Connections
{
    public class FollowPoint : PoolableDrawable, IAnimationTimeReference
    {
        private const float width = 8;

        public override bool RemoveWhenNotAlive => false;

        public FollowPoint()
        {
            Origin = Anchor.Centre;

            InternalChild = new CircularContainer
            {
                Masking = true,
                AutoSizeAxes = Axes.Both,
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Colour = Color4.White.Opacity(0.2f),
                    Radius = 4,
                },
                Child = new Box
                {
                    Size = new Vector2(width),
                    Blending = BlendingParameters.Additive,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Alpha = 0.5f,
                }
            };
        }

        public Bindable<double> AnimationStartTime { get; } = new BindableDouble();
    }
}
