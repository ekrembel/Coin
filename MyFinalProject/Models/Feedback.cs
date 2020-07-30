using System;
namespace MyFinalProject.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Reason { get; set; }

        public Feedback()
        {
        }

        public Feedback(string reason)
        {
            Reason = reason;
        }
    }
}
