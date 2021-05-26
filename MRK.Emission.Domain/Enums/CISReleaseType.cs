using System.ComponentModel;

namespace MRK.Emission.Domain.Enums
{
    public enum CISReleaseType
    {
        [Description("НЕТ")]
        NONE,
        [Description("ПРОИЗВОДСТВО")]
        PRODUCTION,
        [Description("ИМПОРТ")]
        IMPORT,
        [Description("ОСТАТКИ")]
        REMAINS
    }
}
