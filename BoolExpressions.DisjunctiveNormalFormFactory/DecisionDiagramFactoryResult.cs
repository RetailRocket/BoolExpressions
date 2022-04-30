namespace BoolExpressions.DisjunctiveNormalFormFactory
{
    using System.Collections.Generic;
    using DecisionDiagrams;

    internal class DecisionDiagramFactoryResult<T>
        where T : notnull
    {
        public DecisionDiagramFactoryResult(
            DD decisionDiagram,
            Dictionary<DD, T> varTable)
        {
            this.DecisionDiagram = decisionDiagram;
            this.VarTable = varTable;
        }

        public DD DecisionDiagram { get; }

        public Dictionary<DD, T> VarTable { get; }
    }
}