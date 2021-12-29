using UnityEngine;

public class WallBehavior : MonoBehaviour {
    private void Start() {
        GameManager.DestroyEveryWall += DestroyMyself;
    }

    private void DestroyMyself() {
        GameManager.DestroyEveryWall -= DestroyMyself;
        Destroy(gameObject);
    }
}
