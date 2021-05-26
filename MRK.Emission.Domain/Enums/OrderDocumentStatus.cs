using System.ComponentModel;

namespace MRK.Emission.Domain.Enums
{
    public enum OrderDocumentStatus
    {
        [Description("Не существует")]
        NOTEXISTS,
        [Description("Создан")]
        CREATED,
        [Description("Отправлен в СУЗ")]
        POSTED,
        [Description("Обработан")]
        COMPLITED,
        [Description("Отменён")]
        CANCELLED
    }
}
