using Dolittle.Specs.Concepts.given;

namespace Dolittle.Specs.Concepts.for_ConceptAs.given
{

    public class comparable_concepts  : test_concepts 
    {
        public const int LESS_THAN = -1;
        public const int EQUAL = 0;
        public const int GREATER_THAN = 1;
        protected static int compare_least_to_most;
        protected static int compare_least_to_middle;
        protected static int compare_least_to_self;
        protected static int compare_middle_to_least;
        protected static int compare_middle_to_most;
        protected static int compare_middle_to_self; 
        protected static int compare_most_to_least;
        protected static int compare_most_to_middle;
        protected static int compare_most_to_self; 
        protected static int compare_most_to_another_instance_of_most; 
    }
}