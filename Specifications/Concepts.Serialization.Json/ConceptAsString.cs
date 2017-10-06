﻿using System;
using doLittle.Concepts;

namespace doLittle.Concepts.Serialization.Json.Specs
{
    public class ConceptAsString : ConceptAs<String>
    {
        public static implicit operator ConceptAsString(string value)
        {
            return new ConceptAsString { Value = value };
        }
    }
}