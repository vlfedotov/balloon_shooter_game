using UnityEngine;

public class Balloon : MonoBehaviour {
    
    public delegate void ClickAction(float size);
    public static event ClickAction OnClick;
    
    public float speed;
    public float size;

    private float _ceilLimit;

    void Start() {
        transform.localScale = Vector3.one * size;
        _ceilLimit = Camera.main.orthographicSize;
    }

    public void SetColor(Color color) {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = color;
    }
    
    void Update() {
        transform.Translate(transform.up * speed * Time.deltaTime);

        if (transform.position.y - size > _ceilLimit) {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown() {
        if (OnClick != null)
            OnClick(size);
        Destroy(gameObject);
    }
}