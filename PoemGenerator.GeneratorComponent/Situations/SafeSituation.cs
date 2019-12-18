using System.Collections.Generic;
using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent.Situations
{
    public class SafeSituation: Situation
    {
        protected override IEnumerable<IReadOnlyNode> GetRelevantToSituationActions()
        {
            return Action.To(Relations.Action)
                .Where(x => x.FromIsANested().Any(y => y.Name == Nodes.SafeSituation));
        }
    }
}