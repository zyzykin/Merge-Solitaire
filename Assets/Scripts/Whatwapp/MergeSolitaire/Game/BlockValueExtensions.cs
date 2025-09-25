namespace Whatwapp.MergeSolitaire.Game
{
    public static class BlockValueExtensions
    {
        public static string Symbol(this BlockValue value) => value switch
        {
            BlockValue.Ace => "A",
            BlockValue.Two => "2",
            BlockValue.Three => "3",
            BlockValue.Four => "4",
            BlockValue.Five => "5",
            BlockValue.Six => "6",
            BlockValue.Seven => "7",
            BlockValue.Eight => "8",
            BlockValue.Nine => "9",
            BlockValue.Ten => "10",
            BlockValue.Jack => "J",
            BlockValue.Queen => "Q",
            BlockValue.King => "K",
            BlockValue.Bomb => "",
            _ => throw new System.ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}