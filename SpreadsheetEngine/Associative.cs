namespace CptS321
{
    public abstract partial class OperatorNode
    {
        /// <summary>
        /// Enum to mark associativity.
        /// </summary>
        public enum Associative
        {
            /// <summary>
            /// Right Associative.
            /// </summary>
            Right,
            /// <summary>
            /// Left Associative.
            /// </summary>
            Left
        };
    }
}
