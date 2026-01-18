public interface IMetric
{
    (string, double) Evaluate(List<Document> doc);
}