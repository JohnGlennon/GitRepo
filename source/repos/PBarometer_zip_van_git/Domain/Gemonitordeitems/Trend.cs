using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gemonitordeitems
{
    public enum Trend
    {
        [Display(Name = "Dalend")]
        DOWN = 0,
        [Display(Name = "Neutraal")]
        NEUTRAL = 1,
        [Display(Name = "Stijgend")]
        UP = 2
    }
}
