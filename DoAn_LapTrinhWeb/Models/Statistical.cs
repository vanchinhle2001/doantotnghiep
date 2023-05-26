using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    

        [Table("Statistical")]
        public class Statistical
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int statistical_id { get; set; }

            [Required] public string name { get; set; }

            
            public double YearTotal { get; set; }
            public double MonthTotal { get; set; }

            public double DayTotal { get; set; }

            public int view { get; set; }

            [Required] [StringLength(1)] public string status { get; set; }

            [Required] [StringLength(20)] public string create_by { get; set; }

            public DateTime create_at { get; set; }

            [Required] [StringLength(20)] public string update_by { get; set; }

            public DateTime? update_at { get; set; }
        }
 }
