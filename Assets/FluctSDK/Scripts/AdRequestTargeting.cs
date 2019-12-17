
using System;
namespace Fluct
{
    public enum Gender
    {
        unknown = 0,
        male = 1,
        female = 2
    }


    public class AdRequestTargeting
    {
        public string UserId { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public int Age { get; set; }

        public AdRequestTargeting(string userId = null)
        {
            this.UserId = userId;
        }

        public string getBirthdayString() {
            return (this.Birthday == null) ? null : ((DateTime)this.Birthday).ToString("yyyy-MM-dd");
        }
    }
}
