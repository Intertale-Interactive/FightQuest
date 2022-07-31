using UnityEngine;
using UnityEngine.UI;

public class LootAtFollow : MonoBehaviour
{
    public Transform enemy;
    public Transform ui;

    void Update()
    {
        ui.position = new Vector2(enemy.position.x, enemy.position.y);
    }
}
