namespace SpellCheck.Models
{
    internal class InterpretationData
    {
        public List<string> TranslateWords {  get; set; }
        
        public List<string> OriginalWords { get; set; }


        public InterpretationData(List<string> translate, List<string> words)
        { 
            TranslateWords = translate;
            OriginalWords = words;
        }
    }
}
