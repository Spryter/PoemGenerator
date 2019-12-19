using System.Linq;
using PoemGenerator.GeneratorComponent.Constants;
using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent.Helpers
{
    public static class OntologyHelper
    {
        public static IReadOnlyNode GetDangerousNode(IReadOnlyRelation relation)
        {
            return relation.From.ToRelations
                .First(x => x.Name == Relations.Action || x.Name == Relations.Agent || x.Name == Relations.Object ||
                            x.Name == Relations.Locative).From.FromIsANested()
                .Any(x => x.Name == Nodes.DangerousSituation)
                ? relation.From
                : relation.To;
        }
    }
}