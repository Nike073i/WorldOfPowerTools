using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Extensions
{
    public static class ActionsExtensions
    {
        public static bool IsSet(this Actions actions, Actions actionToTest)
        {
            if (actionToTest == Actions.None) throw new ArgumentOutOfRangeException(nameof(actionToTest), "Значение не может быть 0");
            return (actions & actionToTest) == actionToTest;
        }

        public static Actions Set(this Actions actions, Actions setActions)
        {
            return actions | setActions;
        }

        public static Actions Clear(this Actions actions, Actions clearActions)
        {
            return actions & ~clearActions;
        }
    }
}