using System;

namespace Dolittle.Serialization.Protobuf.for_Serializer
{
    public class simple_object_with_primitives
    {
        public static MessageDescription message_description = 
            MessageDescription.DefaultFor<simple_object_with_primitives>();

        public Guid a_guid { get; set; }
        public int an_integer { get; set; }
        public float a_float { get; set; }
        public double a_double { get; set; }
        public string a_string {  get; set; }
        public DateTime a_date_time {  get; set; }
        public DateTimeOffset a_date_time_offset {  get; set; }
    }
}