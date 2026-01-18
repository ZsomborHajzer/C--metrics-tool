public class LinesOfCode : IMetric
{
    private int _lineCount;
    private readonly Func<List<Document>, (string, double)> _func;

    public LinesOfCode()
    {
        _func = Implementation;
    }

    public (string, double) Evaluate(List<Document> docs)
    {
        return _func(docs);
    }

    private (string, double) Implementation(List<Document> docs)
    {
        _lineCount = 0;

        foreach (var doc in docs)
        {
            _lineCount += doc.DocumentLines.Count;
        }

         return ($"Lines of Code (including comments): {_lineCount}", _lineCount);
    }
}

