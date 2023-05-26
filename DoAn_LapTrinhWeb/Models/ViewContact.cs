using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    public class ViewContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int contact_id { get; set; }

        [Required] public string name { get; set; }

        [Required(ErrorMessage = "Nhập số điện thoại")]
        [StringLength(10, ErrorMessage = "Số điện thoại phải đúng 10 chữ số", MinimumLength = 10)]
        public string phone { get; set; }
        [Required(ErrorMessage = "Nhập Email")]
        [StringLength(100, ErrorMessage = "Email tối thiểu 6 ký tự", MinimumLength = 6)]
        public string email { get; set; }

        [Required] public string content { get; set; }


        [Required] [StringLength(1)] public string status { get; set; }

        [Required] [StringLength(20)] public string create_by { get; set; }

        public DateTime create_at { get; set; }

        [Required] [StringLength(20)] public string update_by { get; set; }

        public DateTime? update_at { get; set; }
    }
}