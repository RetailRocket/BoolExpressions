namespace BoolExpressions.DisjunctiveNormalFormFactory
{
    using System.Collections.Generic;
    using BoolExpressions.DisjunctiveNormalForm;
    using BoolExpressions.DisjunctiveNormalForm.Operation;
    using BoolExpressions.NonCanonicalForm;
    using DecisionDiagrams;
    using Optional;

    public static class Factory
    {
        public static Option<DnfExpression<T>> Build<T>(
            INcfExpression<T> ncfExpression)
            where T : notnull
        {
            var manager = new DDManager<BDDNode>(new BDDNodeFactory());

            var decisionDiagramFactoryResult = DecisionDiagramFactory
                .Build(
                    manager: manager,
                    ncfExpression: ncfExpression);

            return BuildOrEmpty(
                manager: manager,
                rootOfDD: decisionDiagramFactoryResult.DecisionDiagram,
                varTable: decisionDiagramFactoryResult.VarTable);
        }

        private static Option<DnfExpression<T>> BuildOrEmpty<T>(
            DD rootOfDD,
            IReadOnlyDictionary<DD, T> varTable,
            DDManager<BDDNode> manager) where T : notnull
        {
            if (rootOfDD.IsConstant())
            {
                return Option.None<DnfExpression<T>>();
            }

            var variable = varTable[rootOfDD];

            var left = Build(
                new ImmutableNotEmptyHashSet<IDnfVariable<T>>(
                    head: new DnfNotVariable<T>(variable)),
                manager,
                varTable,
                manager.Low(rootOfDD));

            var right = Build(
                new ImmutableNotEmptyHashSet<IDnfVariable<T>>(
                    new DnfVariable<T>(variable)),
                manager,
                varTable,
                manager.High(rootOfDD));

            return Option.Some(
                value: new DnfExpression<T>(
                    blockSet: new ImmutableNotEmptyHashSet<DnfBlock<T>>(
                        first: left,
                        second: right)));
        }

        private static ImmutableNotEmptyHashSet<DnfBlock<T>> Build<T>(
            ImmutableNotEmptyHashSet<IDnfVariable<T>> elementSet,
            DDManager<BDDNode> manager,
            IReadOnlyDictionary<DD, T> varTable,
            DD node)
            where T : notnull
        {
            if (node.IsConstant())
            {
                var variable = varTable[node];

                var left = Build(
                    new ImmutableNotEmptyHashSet<IDnfVariable<T>>(
                        head: new DnfNotVariable<T>(variable),
                        tail: elementSet),
                    manager,
                    varTable,
                    manager.Low(node));

                var right = Build(
                    new ImmutableNotEmptyHashSet<IDnfVariable<T>>(
                        head: new DnfVariable<T>(variable),
                        tail: elementSet),
                    manager,
                    varTable,
                    manager.High(node));

                return new ImmutableNotEmptyHashSet<DnfBlock<T>>(
                    first: left,
                    second: right);
            }

            return new ImmutableNotEmptyHashSet<DnfBlock<T>>(
                head: new DnfBlock<T>(elementSet));
        }
    }
}