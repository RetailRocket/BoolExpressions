namespace BoolExpressions.DisjunctiveNormalFormFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BoolExpressions.NonCanonicalForm;
    using DecisionDiagrams;

    internal static class DecisionDiagramFactory
    {
        public static DecisionDiagramFactoryResult<T> Build<T>(
            DDManager<BDDNode> manager,
            INcfExpression<T> ncfExpression)
            where T : notnull
        {
            var varTable = new Dictionary<T, VarBool<BDDNode>>();

            return new DecisionDiagramFactoryResult<T>(
                decisionDiagram: Build(
                    ncfExpression,
                    varTable,
                    manager),
                varTable: varTable.ToDictionary(
                    keySelector: kv => kv.Value.Id(),
                    elementSelector: kv => kv.Key));
        }

        private static DD Build<T>(
            INcfExpression<T> ncfExpression,
            Dictionary<T, VarBool<BDDNode>> varTable,
            DDManager<BDDNode> manager)
            where T : notnull
        {
            return ncfExpression switch
            {
                NcfVariable<T> v => varTable
                    .GetOrCreate(
                        key: v.Value,
                        creator: manager.CreateBool)
                    .Id(),
                NcfAndBlock<T> andBlock => manager.And(
                    Build(andBlock.TermA, varTable, manager),
                    Build(andBlock.TermB, varTable, manager)),
                NcfOrBlock<T> orBlock => manager.Or(
                    Build(orBlock.TermA, varTable, manager),
                    Build(orBlock.TermB, varTable, manager)),
                NcfNot<T> not => manager.Not(
                    Build(
                        ncfExpression: not.NcfExpression,
                        varTable: varTable,
                        manager: manager)),
                _ => throw new ArgumentException(message: "pattern matching in C# is sucks",
                    paramName: nameof(ncfExpression))
            };
        }
    }
}