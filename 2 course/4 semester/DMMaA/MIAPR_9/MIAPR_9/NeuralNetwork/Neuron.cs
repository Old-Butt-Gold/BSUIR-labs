namespace MIAPR_9.NeuralNetwork;

[Serializable]
public class Neuron
{
    [Obsolete("For XML", true)]
    public Neuron() { }
    
    public List<int> Weight { get; init; }

    public Neuron(int vectorSize) =>
        Weight = Enumerable.Range(0, vectorSize)
            .Select(_ => new Random().Next(10)).ToList();


    public int GetAnswer(IEnumerable<int> element) => element.Select((x, i) => x * Weight[i]).Sum();

    public void Teaching(List<int> element, int factor)
    {
        for (var i = 0; i < Weight.Count; i++)
        {
            Weight[i] += factor * element[i];
        }
    }
}