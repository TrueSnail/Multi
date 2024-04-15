using System;

public class StatsChange<Stats>
{
    public Action<Stats> Modification;
    public int Order;
    public object Source;

    public StatsChange(Action<Stats> modification, int order, object source)
    {
        Modification = modification;
        Order = order;
        Source = source;
    }
}
