using PoemGenerator.GeneratorComponent.Extensions;
using PoemGenerator.OntologyModel.Abstractions;
using PoemGenerator.OntologyModel.Collections;

namespace PoemGenerator.GeneratorComponent.Situations
{
    public class DangerousSituation: Situation
    {
        public override IReadOnlyNodeCollection GetNodes()
        {
            return new[] {Action, Object, Locative}.ToNodeCollection();
        }

        public override IReadOnlyRelationCollection GetRelations()
        {
            return new RelationCollection();
        }
    }
}