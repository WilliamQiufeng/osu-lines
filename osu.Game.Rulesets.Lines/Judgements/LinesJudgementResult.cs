using System;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Lines.Objects;

namespace osu.Game.Rulesets.Lines.Judgements
{
    public class LinesJudgementResult : JudgementResult
    {
        public LinesJudgementResult(LinesHitObject hitObject, Judgement judgement)
            : base(hitObject, judgement)
        {
        }
        /// <summary>
        /// The <see cref="DivaHitObject"/> that was judged.
        /// </summary>
        public new LinesHitObject HitObject => (LinesHitObject)base.HitObject;


        /// <summary>
        /// The judgement which this <see cref="DivaJudgementResult"/> applies for.
        /// </summary>
        public new Judgement Judgement => base.Judgement;
    }
}
