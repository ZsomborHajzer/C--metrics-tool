using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

class AverageFunctionSize : IMetric
{

    public (string, double) Evaluate(List<Document> docs)
    {
        return Implementation(docs);
    }

    private (string, double) Implementation(List<Document> docs)
    {
        List<int> functionSizes = new List<int>();
        float avg = 0;

        foreach(var doc in docs)
        {
            var functions = doc.SyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach(var function in functions)
            {
                var text = function.GetText();
                functionSizes.Add(text.Lines.Count);
            }
        }
        avg = functionSizes.Sum() / functionSizes.Count();
        return ($"The average function size is {avg}",avg);
    }

}
