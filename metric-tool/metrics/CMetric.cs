using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


public class CMetric : IMetric
{
    double _weightOfAlgorithmicComplexity = 1;
    double _weightOfStructuralComplexity = 1;
    double _weightOfFeatureDensity = 1;


    //That's called a crime, never do that V

    IMetric _asyncAwaitCounter = new AsyncAwaitCounter();
    IMetric _avearageClassSize = new AverageClassSize();
    IMetric _avearageFunctionSize = new AverageFunctionSize();
    IMetric _concurrencyUsage = new ConcurrencyUsage();
    IMetric _cyclomaticComplexity = new CyclomaticComplexity();
    IMetric _halsteadEffort = new HalsteadEffort();
    IMetric _complexityKeywordDensity = new KeywordDensity();
    IMetric _lambdaExpressions = new LambdaFunctionCounter();
    IMetric _LOC = new LinesOfCode();
    IMetric _nestingDepth = new NestingDepth();
    IMetric _parameterPerFunctionViolation = new ParameterPerFunctionViolation();

    public (string, double) Evaluate(List<Document> docs)
    {
        return Implementation(docs);
    }

    private (string, double) Implementation(List<Document> docs)
    {
        double result = (_weightOfAlgorithmicComplexity * AlgorithmicComplexity(docs) + _weightOfStructuralComplexity * StructuralComplexity(docs) + _weightOfFeatureDensity * FeatureDensity(docs)) / (_weightOfAlgorithmicComplexity + _weightOfStructuralComplexity + _weightOfFeatureDensity);
        return ($"C = {result}", result);
    }

    private double AlgorithmicComplexity(List<Document> docs)
    {
        double weightOfHalsteadEffort = 1;
        double weightOfCyclomaticComplexity = 1;
        double weightOfAvearageNestingDepth = 1;
        double weightOfRecursionDensity = 0; // not implemented

        //Normalize H_E and C_C
        double halsteadNormalizedToLOC = _halsteadEffort.Evaluate(docs).Item2 / _LOC.Evaluate(docs).Item2;
        double cyclomaticNormalizedToLOC = _cyclomaticComplexity.Evaluate(docs).Item2 / _LOC.Evaluate(docs).Item2;

        //According to the formula
        return (weightOfHalsteadEffort * halsteadNormalizedToLOC + cyclomaticNormalizedToLOC * weightOfCyclomaticComplexity + weightOfAvearageNestingDepth * _nestingDepth.Evaluate(docs).Item2) / (weightOfHalsteadEffort + weightOfCyclomaticComplexity + weightOfAvearageNestingDepth + weightOfRecursionDensity);
    }

    private double StructuralComplexity(List<Document> docs)
    {
        double weightOfFunctionSize = 1;
        double weightOfClassSize = 1;
        double weightOfPPFV = 1;
        double weightOfCouplingBetweenObjects = 0; //can't find it :(

        double classSizeNormalizedToLOC = _avearageClassSize.Evaluate(docs).Item2 / _LOC.Evaluate(docs).Item2;
        double parameterViolationNormalized = _parameterPerFunctionViolation.Evaluate(docs).Item2 / _LOC.Evaluate(docs).Item2;

        //According to formula
        return (weightOfClassSize * classSizeNormalizedToLOC + weightOfFunctionSize * _avearageFunctionSize.Evaluate(docs).Item2 + weightOfPPFV * parameterViolationNormalized + weightOfCouplingBetweenObjects) / (weightOfClassSize + weightOfFunctionSize + weightOfPPFV + weightOfCouplingBetweenObjects);
    }

    private double FeatureDensity(List<Document> docs)
    {
        double LOC = _LOC.Evaluate(docs).Item2;
        double weightOfAsync = 1;
        double weightOfLambda = 1;
        double weightOfKeyword = 1;
        double weightOfConcurency = 1;

        double asyncRatio = _asyncAwaitCounter.Evaluate(docs).Item2 / LOC;
        double lambdaRatio = _lambdaExpressions.Evaluate(docs).Item2 / LOC;
        double keywordRatio = _complexityKeywordDensity.Evaluate(docs).Item2 / LOC;
        double concurrencyRatio = _concurrencyUsage.Evaluate(docs).Item2 / LOC;

        return (weightOfAsync * asyncRatio + weightOfLambda * lambdaRatio + weightOfKeyword * keywordRatio + weightOfConcurency * concurrencyRatio) / (weightOfAsync + weightOfLambda + weightOfKeyword + weightOfConcurency);
    }
}
