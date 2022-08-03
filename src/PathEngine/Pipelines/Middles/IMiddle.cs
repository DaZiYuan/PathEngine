namespace PathEngine.Pipelines.Middles
{
    public interface IMiddle
    {
        Payload Input(Payload payload);
    }
}
