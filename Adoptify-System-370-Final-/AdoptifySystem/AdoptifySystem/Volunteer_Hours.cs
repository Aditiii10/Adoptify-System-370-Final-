//namespace AdoptifySystem
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;  //needed for Display annotation
//    using System.ComponentModel;  //DisplayName annotation

//    public partial class Volunteer_Hours
//    {
//        [Display(Name = "Volunteer Hours")]
//        public int Vol_Hours_ID { get; set; }


//        [Display(Name = "Volunteer Work Date")]
//        [Required]
//        public DateTime Vol_workDate { get; set; }


//        [Display(Name = "Volunteer Start Time")]
//        [Required]
//        public DateTime Vol_Start_Time { get; set; }


//        [Display(Name = "Volunteer End Time")]
//        [Required]
//        public DateTime Vol_End_Time { get; set; }


//        [Display(Name = "Volunteer")]
//        public int Vol_ID { get; set; }


//        [Display(Name = "Volunteer Work Type")]
//        public int Vol_WorkType_ID { get; set; }


//        [Display(Name = "Volunteer")]
//        public virtual Volunteer Volunteer { get; set; }


//        [Display(Name = "Volunteer Work Type")]
//        public virtual Volunteer_Work_Type Volunteer_Work_Type { get; set; }
//    }
//}
namespace AdoptifySystem
{
    using System;
    using System.Collections.Generic;

    public partial class Volunteer_Hours
    {
        public int Vol_Hours_ID { get; set; }
        public DateTime Vol_workDate { get; set; }
        public TimeSpan Vol_Start_Time { get; set; }
        public TimeSpan Vol_End_Time { get; set; }
        public Nullable<int> Vol_ID { get; set; }
        public Nullable<int> Vol_WorkType_ID { get; set; }

        public virtual Volunteer Volunteer { get; set; }
        public virtual Volunteer_Work_Type Volunteer_Work_Type { get; set; }
    }
}
