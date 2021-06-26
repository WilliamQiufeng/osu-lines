// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Lines.Replays
{
    public class LinesReplayFrame : ReplayFrame
    {
        public List<LinesAction> Actions = new List<LinesAction>();

        public LinesReplayFrame(LinesAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }
        public LinesReplayFrame(double time, params LinesAction[] actions) : base(time)
		{
            Actions.AddRange(actions);
		}
    }
}
