using System;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Lines.Scores
{
    public class LinesHitWindows : HitWindows
    {
        public override bool IsHitResultAllowed(HitResult result)
        {
            switch (result)
            {
                case HitResult.Perfect:
                case HitResult.Great:
                case HitResult.Good:
                case HitResult.Ok:
                case HitResult.Meh:
                case HitResult.Miss:
                    return true;
            }
            return false;
        }
    }
}
