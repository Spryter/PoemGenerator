using System.Collections.Generic;
using PoemGenerator.OntologyModel.Abstractions;

namespace PoemGenerator.GeneratorComponent
{
    public class RelevantNodes
    {
        public IEnumerable<IReadOnlyNode> Agents { get; set; }
        
        public IEnumerable<IReadOnlyNode> Actions { get; set; }
        
        public IEnumerable<IReadOnlyNode> Objects { get; set; }
        
        public IEnumerable<IReadOnlyNode> Locatives { get; set; }
    }
}