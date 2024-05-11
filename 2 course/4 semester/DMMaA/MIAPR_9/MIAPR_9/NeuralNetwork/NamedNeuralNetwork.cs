namespace MIAPR_9.NeuralNetwork;

[Serializable]
public class NamedNeuralNetwork
{
    [Obsolete("For XML", true)]
    public NamedNeuralNetwork() { }

    public NeuralNetwork NeuralNetwork { get; init; }

    public List<string> NeuronsNames { get; init; }

    public NamedNeuralNetwork(int vectorSize, List<string> neuronsNames)
    {
        NeuronsNames = neuronsNames;
        NeuralNetwork = new(vectorSize, neuronsNames.Count);
    }

    public void Teaching(List<int> element, string correctClassName)
    {
        var correctClass = NeuronsNames.FindIndex(x => x == correctClassName);
        if (correctClass == -1) 
            throw new ArgumentException("Неправильное имя класса");
        NeuralNetwork.Teaching(element, correctClass);
    }

    public string GetAnswer(List<int> element) => NeuronsNames[NeuralNetwork.GetAnswer(element)];
}