namespace MIAPR_9.NeuralNetwork;

[Serializable]
public class NeuralNetwork
{
    [Obsolete("For XML", true)]
    public NeuralNetwork() { }
    
    const int Threshold = 70;

    public List<Neuron> Neurons { get; init; }

    public NeuralNetwork(int vectorSize, int neuronsCount)
    {
        Neurons = Enumerable.Range(0, neuronsCount)
            .Select(_ => new Neuron(vectorSize)).ToList();
    }

    public void Teaching(List<int> element, int correctClass)
    {
        var neuronsThresholdAnswers = GetNeuronsThresholdAnswers(element);
        while (!IsCorrectAnswer(neuronsThresholdAnswers, correctClass))
        {
            for (var i = 0; i < Neurons.Count; i++)
            {
                var neuronFactor = GetNeuronFactor(neuronsThresholdAnswers[i], i, correctClass);
                Neurons[i].Teaching(element, neuronFactor);
            }
            neuronsThresholdAnswers = GetNeuronsThresholdAnswers(element);
        }
    }

    List<bool> GetNeuronsThresholdAnswers(IEnumerable<int> element) => 
        Neurons.Select(x => x.GetAnswer(element) > Threshold).ToList();

    bool IsCorrectAnswer(IReadOnlyList<bool> neuronsThresholdAnswers, int correctClass) => 
        neuronsThresholdAnswers[correctClass] && neuronsThresholdAnswers.Count(x => x) == 1;

    int GetNeuronFactor(bool neuronThresholdAnswer, int neuronNumber, int correctClass) =>
        neuronThresholdAnswer ? (neuronNumber == correctClass ? 0 : -1) : (neuronNumber == correctClass ? 1 : 0);

    public int GetAnswer(List<int> element)
    {
        var neuronsAnswer = Neurons.Select(x => x.GetAnswer(element)).ToList();
        return neuronsAnswer.IndexOf(neuronsAnswer.Max());
    }
}