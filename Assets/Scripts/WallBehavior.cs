using UnityEngine;

public class WallBehavior : MonoBehaviour {
    private void Start() {
        GameManager.DestroyEveryWall += DestroyMyself;
        GameManager.OnEndScene += Unsubscribe;
    }

    private void Unsubscribe() {
        GameManager.OnEndScene += Unsubscribe;
        GameManager.DestroyEveryWall -= DestroyMyself;
    }

    private void DestroyMyself() {
        GameManager.DestroyEveryWall -= DestroyMyself;
        Destroy(gameObject);
    }
}
