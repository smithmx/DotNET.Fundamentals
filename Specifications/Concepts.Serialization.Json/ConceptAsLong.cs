﻿using doLittle.Concepts;

namespace doLittle.Concepts.Serialization.Json.Specs
{
    public class ConceptAsLong : ConceptAs<long>
    {
        public static implicit operator ConceptAsLong(long value)
        {
            return new ConceptAsLong { Value = value };
        }
    }
}