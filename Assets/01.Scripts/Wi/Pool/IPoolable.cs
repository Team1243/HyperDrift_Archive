using UnityEngine;
/// <summary>
/// 이 인터페이스를 상속 받아야 풀링이 가능하다.
/// </summary>
public interface IPoolable
{
    public void Initialize();
    public GameObject gameObject { get; }
}
