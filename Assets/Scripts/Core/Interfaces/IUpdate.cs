/// <summary>
/// 心跳接口，会每帧都执行
/// </summary>
public interface IUpdate
{
    void Update(float deltaTime);
}
