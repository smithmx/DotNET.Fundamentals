﻿using System;
using Machine.Specifications;

namespace Dolittle.Dynamic.Specs.for_WriteOnceExpandoObject
{
    [Subject(typeof(WriteOnceExpandoObject))]
    public class when_adding_value_to_dictionary_after_creation : given.a_write_once_expando_object_without_values
    {
        protected static Exception exception;
        Because of = () => exception = Catch.Exception(() => values["Something"] = 5);
#pragma warning disable 0169        
        Behaves_like<a_read_only_container> a_read_only_container;
#pragma warning restore 0169        
    }
}
