using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.AtnCompletion
{
    class TransitionWrapper
    {
        public TransitionWrapper(ATNState state, Transition transition)
        {
            this.state = state;
            this.transition = transition;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((state == null) ? 0 : state.GetHashCode());
            result = prime * result + ((transition == null) ? 0 : transition.GetHashCode());
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null && !(obj is TransitionWrapper))
                return false;

            var other = obj as TransitionWrapper;
            if ((state == null && other.state != null) || (state != null && other.state == null))
                return false;
            if ((transition == null && other.transition != null) || (transition != null && other.transition == null))
                return false;

            return (state == other.state || state.Equals(other.state)) && (transition == other.transition || transition.Equals(other.transition));
        }

        private ATNState state;
        private Transition transition;
    }
}
