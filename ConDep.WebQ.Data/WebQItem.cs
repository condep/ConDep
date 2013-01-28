using System;

namespace ConDep.WebQ.Data
{
    public class WebQItem
    {
        public string Id { get; set; }
        public DateTime? StartTime { get; set; }
        public int Position { get; set; }
        public string Environment { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}