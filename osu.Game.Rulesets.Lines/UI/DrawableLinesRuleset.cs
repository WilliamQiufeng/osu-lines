// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Lines.Objects;
using osu.Game.Rulesets.Lines.Objects.Drawables;
using osu.Game.Rulesets.Lines.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Lines.UI
{
    [Cached]
    public class DrawableLinesRuleset : DrawableRuleset<LinesHitObject>
    {
        public DrawableLinesRuleset(LinesRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override Playfield CreatePlayfield() => new LinesPlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new LinesPlayfieldAdjustmentContainer { AlignWithStoryboard = true };

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new LinesFramedReplayInputHandler(replay);

        public override DrawableHitObject<LinesHitObject> CreateDrawableRepresentation(LinesHitObject h) => new DrawableLinesHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new LinesInputManager(Ruleset?.RulesetInfo);
    }
}
