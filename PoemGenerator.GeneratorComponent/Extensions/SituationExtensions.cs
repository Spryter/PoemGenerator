using PoemGenerator.GeneratorComponent.Situations;

namespace PoemGenerator.GeneratorComponent.Extensions
{
    public static class SituationExtensions
    {
        public static SafeSituation ToSafeSituation(this Situation situation)
        {
            return new SafeSituation
            {
                Action = situation.Action,
                Agent = situation.Agent,
                Object = situation.Object,
                Locative = situation.Locative
            };
        }
        
        public static DangerousSituation ToDangerousSituation(this Situation situation)
        {
            return new DangerousSituation
            {
                Action = situation.Action,
                Agent = situation.Agent,
                Object = situation.Object,
                Locative = situation.Locative
            };
        }
    }
}