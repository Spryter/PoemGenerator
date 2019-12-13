using System;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.App
{
    public class OntologyViewModel
    {
        public event Action<IReadOnlyNodeCollection, IReadOnlyRelationCollection> ColorizeGraph;

        public void UpdateGraphColoring(IReadOnlyNodeCollection nodes, IReadOnlyRelationCollection relations)
        {
            ColorizeGraph?.Invoke(nodes, relations);
        }
    }
}