using System;
using Machine.Specifications;

namespace doLittle.Applications.for_ApplicationStructureFragment
{
    public class when_initialized_with_type_not_belonging_to_parent
    {
        static Exception result;

        Because of = () => result = Catch.Exception(() =>
                                        new ApplicationStructureFragment(
                                            typeof(FragmentWithoutBelonging),
                                            new ApplicationStructureFragment(typeof(Fragment))
                                        )
                                    );

        It should_throw_application_structure_fragment_must_belong_to_parent = () => result.ShouldBeOfExactType<ApplicationStructureFragmentMustBelongToParent>();
    }
}