using UnityEngine;
using System.Collections;

public class SeedParticle : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 motion;
    private float alpha = 1f;
    private float gravity = -2f;
    private float lifeTime = 1f;
    private float timer = 0f;

    private ObjectPoolManager<SeedParticle> poolRef;

    public void Init(ObjectPoolManager<SeedParticle> pool)
    {
        poolRef = pool;
        sr = GetComponent<SpriteRenderer>();
    }

    public void Launch(Vector3 startPos)
    {
        transform.position = startPos;
        alpha = 1f;
        timer = 0f;
        motion = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(1f, 1.5f));
        sr.color = Color.white;
        gameObject.SetActive(true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Rơi dần
        motion.y += gravity * Time.deltaTime;
        transform.position += (Vector3)(motion * Time.deltaTime);

        // Mờ dần
        alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);
        sr.color = new Color(1, 1, 1, alpha);

        if (timer >= lifeTime)
            ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        if (poolRef != null)
            poolRef.Release(this);
        else
            Destroy(gameObject);
    }
}