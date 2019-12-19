using System.Linq;
using System.Text;
using LingvoNET;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Situations;

namespace PoemGenerator.GeneratorComponent
{
    public class Morpher
    {
        public string GetMorphedSituationString(Situation situation)
        {
            var builder = new StringBuilder();
            builder.Append($"{situation.Agent} ");
            
            if (situation.Action.Name != EmptyOntologyNode.Name)
            {
                var action = Verbs.FindSimilar(situation.Action.Name);
                if (action != null)
                {
                    var agent = Nouns.FindOne(situation.Agent.Name);
                    var agentGender = agent?.Gender ?? Gender.M;
                    var morphedAction = action.Past(Voice.Active, agentGender);
                    builder.Append($"{morphedAction} ");
                }
            }

            if (situation.Object.Name != EmptyOntologyNode.Name)
            {
                var _object = Nouns.FindSimilar(situation.Object.Name);
                if (_object != null)
                {
                    var morphedObject = _object[Case.Accusative];
                    builder.Append($"{morphedObject} ");
                }
            }

            if (situation.Locative.Name != EmptyOntologyNode.Name)
            {
                var locative = Nouns.FindSimilar(situation.Locative.Name);
                if (locative != null)
                {
                    var preposition = situation.Locative.FromRelations
                        .Where(x => x.Name == Relations.Prepostion)
                        .Select(x => x.To.Name)
                        .FirstOrDefault();
                    var morphedLocative = $"{preposition} {locative[Case.Locative]}";
                    builder.Append($"{morphedLocative} ");
                }
            }
            
            return builder.ToString();
        }
    }
}