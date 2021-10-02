using Constants;

public class Score
{
    public ElementTypes Element;
    public int Value;
    public int Combo;

    public Score(ElementTypes element, int value)
    {
        Element = element;
        Value = value;
        Combo = (value % 2) + 1;
    }
}
