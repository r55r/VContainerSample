using JetBrains.Annotations;

namespace Service
{
    [UsedImplicitly]
    public sealed class DebtService
    {
        public bool AllowOverdraft { get; set; }
    }
}