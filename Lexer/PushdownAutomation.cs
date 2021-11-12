using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    class PushdownAutomation<State, Symbol> where State : Enum
    {
        private static readonly State initialState = (State) typeof(State)
            .GetFields()
            .First(fInfo => fInfo.IsDefined(typeof(InitialAttribute), false))
            .GetValue(null);

        private static readonly IEnumerable<State> finalStates = typeof(State)
            .GetFields()
            .Where(fInfo => fInfo.IsDefined(typeof(FinalAttribute), false))
            .Select(fInfo => fInfo.GetValue(null))
            .Cast<State>();

        private State currentState = initialState;
        private readonly Stack<Symbol> charStack = new();
        private readonly TransitionFunction transitionFunction;

        public delegate State TransitionFunction(State state, Stack<Symbol> stack, Queue<Symbol> input);

        public PushdownAutomation(TransitionFunction transitionFunction)
            => this.transitionFunction = transitionFunction;

        public State Run(IEnumerable<Symbol> input)
        {
            var inputRibbon = new Queue<Symbol>(input);

            while (!finalStates.Contains(currentState))
            {
                currentState = transitionFunction(currentState, charStack, inputRibbon);
            }

            return currentState;
        }
    }
}
