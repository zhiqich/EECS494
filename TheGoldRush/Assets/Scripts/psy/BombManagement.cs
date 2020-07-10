using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManagement : MonoBehaviour
{
    public AudioClip BombExplodeSoundclip;
    public float BombDelay = 3;
    public GameObject ExplosionPrefab;
    public int BombDamage = 3; 
    public GameObject owner;
    public Sprite BlueBombSprite, RedBombSprite;
    
    int BombSize;
    float timer;
    BombermanManagement ownerbm;
    bool exploded;
    PlayerBrain.Color color;
    HashSet<GameObject> CollidedPlayers = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        timer = BombDelay;
        exploded = false;

        // get all players in the same grid
        Collider2D[] colls = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D coll in colls)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                CollidedPlayers.Add(coll.gameObject);
                // ignore collision
                Physics2D.IgnoreCollision(coll.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            }
        }
    }

    void SetOwner(GameObject bombowner)
    {
        owner = bombowner;
        color = bombowner.GetComponent<PlayerBrain>().color;
        if (color == PlayerBrain.Color.Blue)
        {
            GetComponent<SpriteRenderer>().sprite = BlueBombSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = RedBombSprite;
        }
        ownerbm = bombowner.GetComponent<BombermanManagement>();

        // add owner to CollidedPlayers
        CollidedPlayers.Add(owner);
        Physics2D.IgnoreCollision(owner.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
    }

    void SetBombSize(int sz)
    {
        BombSize = sz;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Explode();
        }

        // reactivate collision with player if left
        if (CollidedPlayers.Count != 0)
        {
            CollidedPlayers.RemoveWhere(WalkedAway);
        }
    }

    public void Explode()
    {
        if (exploded)
            return;
        exploded = true;

        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(BombExplodeSoundclip, Camera.main.transform.position);
        InstantiateExplosion(transform.position);
        for (int i = 1; i <= BombSize; i++)
        {
            InstantiateExplosion(new Vector2(transform.position.x - i, transform.position.y));
            InstantiateExplosion(new Vector2(transform.position.x + i, transform.position.y));
            InstantiateExplosion(new Vector2(transform.position.x, transform.position.y - i));
            InstantiateExplosion(new Vector2(transform.position.x, transform.position.y + i));
        }

        ownerbm.BombExplode();
    }

    void InstantiateExplosion(Vector2 pos)
    {
        // check if available
        Collider2D coll = Physics2D.OverlapPoint(pos);
        if (coll != null)
        {
            if (coll.gameObject.CompareTag("wall"))
            {
                return;
            }
            if (coll.gameObject.CompareTag("BombExplosion"))
            {
                return;
            }
        }

        GameObject explosionInstance = Instantiate(ExplosionPrefab, pos, Quaternion.identity);
        explosionInstance.GetComponent<ExplosionManagement>().color = owner.GetComponent<PlayerBrain>().color;
        explosionInstance.GetComponent<ExplosionManagement>().ownerID = owner.GetComponent<PlayerBrain>().PlayerID;
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Arrow") || coll.gameObject.CompareTag("Pickaxe"))
        {
            Explode();
        }
    }

    // Predicate used by HashSet.RemoveWhere()
    bool WalkedAway(GameObject player)
    {
        if (Vector2.Distance(player.transform.position, transform.position) >= 1)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            return true;
        }
        return false;
    }
}
