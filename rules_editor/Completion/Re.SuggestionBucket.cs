using Re.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Completion
{
    /// <summary> Class for collection suggestions. </summary>
    class SuggestionBucket
    {
        /// <summary> Constructor. </summary>
        /// <param name="vocabulary"> Vocabulary </param>
        public SuggestionBucket(Antlr4.Runtime.IVocabulary vocabulary)
        {
            Vocabulary = vocabulary;
        }

        /// <summary> Gets collected suggestions. </summary>
        public IReadOnlyCollection<string> Suggestions => mSuggestions;

        /// <summary> Adds a token of the specified type. </summary>
        /// <param name="tokenType"> Token type </param>
        public void Add(int tokenType)
        {
            mSuggestions.Add(GetName(tokenType));
        }

        /// <summary> Adds tokens of the specified types. </summary>
        /// <param name="tokenTypes"> Token types </param>
        public void Add(IEnumerable<int> tokenTypes)
        {
            foreach (var t in tokenTypes)
                mSuggestions.Add(GetName(t));
        }

        /// <summary> Adds string suggestions </summary>
        /// <param name="suggestions"> Suggestions </param>
        public void Add(IEnumerable<string> suggestions)
        {
            foreach (var s in suggestions)
                mSuggestions.Add(s);
        }

        private string GetName(int type)
        {
            return Vocabulary.GetDisplayName(type);
        }

        private Antlr4.Runtime.IVocabulary Vocabulary { get; }
        private HashSet<string> mSuggestions = new HashSet<string>();
    }
}
