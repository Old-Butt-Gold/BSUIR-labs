public class TreeNode<TValue> 
{
    public TreeNode<TValue>? Left, Right;
    public bool RightThreaded { get; set; }
    public TValue Value { get; set; }
    public TreeNode(TValue value) => Value = value;

}