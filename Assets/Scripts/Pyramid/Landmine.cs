interface IFeetDetect
{
    void OnStepOn();
}

public class Landmine : Bomb, IFeetDetect
{
    public void OnStepOn()
    {
        Remove();
    }
}