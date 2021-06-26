// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Lines.Beatmaps;
using osu.Game.Rulesets.Lines.Mods;
using osu.Game.Rulesets.Lines.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Lines
{
    public class LinesRuleset : Ruleset
    {
        public override string Description => "arrows! lines!";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableLinesRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new LinesBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new LinesDifficultyCalculator(this, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.Automation:
                    return new[] { new LinesModAutoplay() };

                default:
                    return new Mod[] { null };
            }
        }

        public override string ShortName => "lines";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.A, LinesAction.Left),
            new KeyBinding(InputKey.L, LinesAction.Right),
            new KeyBinding(InputKey.S, LinesAction.Down),
            new KeyBinding(InputKey.K, LinesAction.Up)
        };

        public override Drawable CreateIcon() => new SpriteText
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Text = ShortName[0].ToString(),
            Font = OsuFont.Default.With(size: 18),
        };
        protected override IEnumerable<HitResult> GetValidHitResults() => new[]
        {
            HitResult.Perfect,
            HitResult.Great,
            HitResult.Good,
            HitResult.Ok,
            HitResult.Meh,
            HitResult.Miss,
        };
    }
}
