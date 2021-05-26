using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MRK.Emission.Domain.Enums
{
    public enum CISStatus
    {
        [Description("Нет")]
        NONE,
        [Description("Эмитирован")]
        EMITTED,
        [Description("Зарезервирован")]
        RESERVED,
        [Description("Распечатан")]
        PRINTED,
        [Description("Нанесён")]
        MARKED,
        [Description("Введён в оборот")]
        INTRODUCED,
        [Description("Выведен из обороа")]
        TURNEDOVER
    }
}
