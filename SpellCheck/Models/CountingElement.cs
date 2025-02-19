
using SpellCheck.Interfaces;

namespace SpellCheck.Models
{
    class CountingElement : BaseElement
    {
        public static int CountingElements { get; private set; } = 0;
        
        public readonly int Id;

        public string Name { get; set; }

        public string Description { get; set; }


        public CountingElement(string name, string description = "Discription wasn't added D:")
        {
            AutoIncrement();

            Id = CountingElements;
            Name = name;
            Description = description;
        }

        private void AutoIncrement()
        {
            CountingElements++;
        }
    }
}
