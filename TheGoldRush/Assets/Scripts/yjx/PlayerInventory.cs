using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    PlayerBrain pb;

    PlayerMove pm;

    public GameObject shieldSprite;
    public bool hasItem = false;
    public bool shield = false;
    public GameObject[] itemList;
    public Material noGold, haveGold;

    public AudioClip coin;

    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBrain>();
        pm = GetComponent<PlayerMove>();
        //numGoldBarText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGetGoldMusic()
    {
        AudioSource.PlayClipAtPoint(coin, transform.position);
    }

    //public Text numGoldBarText;

    public int numGoldBar = 0;
    public Image[] golds;

    //private bool nearBase = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attacked(collision);
        CollectGoldBar(collision);
        //Base(collision, true);
        Score(collision);
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     //Debug.Log("test");
    //     if (pb.stunned)
    //     {
    //         return;
    //     }
    //     if (other.gameObject.CompareTag("GoldBar") && numGoldBar < pb.capacity)
    //     {
    //         numGoldBar++;
    //         goldUpdate();
    //         numGoldBarText.text = numGoldBar.ToString();
    //         Destroy(other.gameObject);
    //     }
    //     if (other.gameObject.CompareTag("wing") && hasItem == false)
    //     {
    //         hasItem = true;
    //         pb.speed += 10;
    //         itemList[0].SetActive(true);
    //         StartCoroutine(WingItem());
    //         Destroy(other.gameObject);
    //     }
    // }

    IEnumerator WingItem()
    {
        yield return new WaitForSeconds(4);
        float currentTime = Time.time;
        float progress = Time.time;
        float blink = Time.time;
        bool active = true;
        while (progress - currentTime <= 3.0f)
        {
            progress = Time.time;
            if (progress - blink >= 0.2f)
            {
                active = !active;
                itemList[0].SetActive(active);
                blink = progress;
            }
            yield return null;
        }
        pb.speed -= 1;
        pm.dust.GetComponent<ParticleSystem>().startSize = 0.2f;
        itemList[0].SetActive(false);
        hasItem = false;
    }
    IEnumerator ShieldItem()
    {
        yield return new WaitForSeconds(17);
        float currentTime = Time.time;
        float progress = Time.time;
        float blink = Time.time;
        bool active = true;
        while (progress - currentTime <= 3.0f)
        {
            if (shield == false)
            {
                itemList[1].SetActive(false);
                break;
            }
            progress = Time.time;
            if (progress - blink >= 0.2f)
            {
                active = !active;
                itemList[1].SetActive(active);
                blink = progress;
            }
            yield return null;
        }
        if (shield == true){
            itemList[1].SetActive(false);
            shield = false;
            hasItem = false;
            shieldSprite.SetActive(false);
        }
    }

    private void Score(Collider2D collision)
    {
        if (pb.stunned || pb.freezed)
        {
            return;
        }
        if (collision.CompareTag("Base") && collision.GetComponent<BaseTrigger>().color == pb.color)
        {
            pb.health = pb.maxHealth;
            pb.slider.value = pb.health;
            if (numGoldBar > 0)
            {
                collision.GetComponentInParent<Score>().PlayerScore(pb.PlayerID, numGoldBar);
                numGoldBar = 0;
                goldUpdate();
                //numGoldBarText.text = numGoldBar.ToString();
            }
        }
    }

    public GameObject goldBarPrefab;

    private void Attacked(Collider2D collision)
    {
        if (pb.invincible)
        {
            return;
        }
        (int damage, int attacker) = GetDamageValue(collision);
        if (damage != -1)
        {
            if (shield){
                itemList[1].SetActive(false);
                shield = false;
                hasItem = false;
                shieldSprite.SetActive(false);
                return;
            }
            Debug.Log("attacked");
            pb.health -= damage;
            StartCoroutine(AttackedFeedback());
            if (pb.health < 0) pb.health = 0;
            pb.slider.value = pb.health;
            if (pb.health == 0)
            {
                pb.Stun(3);
                GameManager.addNewKill(attacker - 1);
                if (numGoldBar > 0)
                {
                    DropGold();
                }
            }
        }
    }

    IEnumerator AttackedFeedback()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void DropGold()
    {
        for (int i = 0; i < numGoldBar; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0).normalized;
            GameObject droppedGoldBar = GameObject.Instantiate(goldBarPrefab, transform.position, Quaternion.identity);
            droppedGoldBar.GetComponent<GoldBrickFly>().target = 0.5f * offset + transform.position;
        }
        numGoldBar = 0;
        goldUpdate();
        //numGoldBarText.text = numGoldBar.ToString();

    }

    private (int, int) GetDamageValue(Collider2D collision)
    {
        if (!pb.stunned)
        {
            if (collision.CompareTag("Pickaxe") && collision.GetComponent<Pickaxe>().owner.GetComponent<PlayerBrain>().color != pb.color)
            {
                int attackerID = collision.GetComponent<Pickaxe>().owner.GetComponent<PlayerBrain>().PlayerID;
                return (Damage.hammer, attackerID);
            }
            else if (collision.CompareTag("Arrow") && collision.GetComponent<Arrow>().color != pb.color)
            {
                int attackerID = collision.GetComponent<Arrow>().ownerID;
                return (Damage.shooter, attackerID);
            }
            else if (collision.CompareTag("BombExplosion") && collision.GetComponent<ExplosionManagement>().color != pb.color)
            {
                int attackerID = collision.GetComponent<ExplosionManagement>().ownerID;
                return (Damage.bomb, attackerID);
            }
            else
            {
                return (-1, 0);
            }
        }
        else
        {
            return (-1, 0);
        }
    }

    private void CollectGoldBar(Collider2D collision)
    {
        if (pb.stunned)
        {
            return;
        }
        if (collision.CompareTag("GoldBar") && numGoldBar < pb.capacity)
        {
            Destroy(collision.gameObject);
            numGoldBar++;
            goldUpdate();
            //numGoldBarText.text = numGoldBar.ToString();
            //AudioSource.PlayClipAtPoint(coin, transform.position);
            PlayGetGoldMusic();
        }
        if (collision.CompareTag("wing") && hasItem == false)
        {
            hasItem = true;
            pb.speed += 1;
            itemList[0].SetActive(true);
            pm.dust.GetComponent<ParticleSystem>().startSize = 0.4f;
            StartCoroutine(WingItem());
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("shield") && hasItem == false)
        {
            hasItem = true;
            shield = true;
            itemList[1].SetActive(true);
            shieldSprite.SetActive(true);
            StartCoroutine(ShieldItem());
            Destroy(collision.gameObject);
        }
    }

    public void goldUpdate()
    {
        Color defaultColor = new Color(0.6f, 0.6f, 0.6f, 0.6f);
        Material m;
        switch (numGoldBar)
        {
            case 0:
                // golds[0].enabled = false;
                // golds[1].enabled = false;
                // golds[2].enabled = false;
                golds[0].color = defaultColor;
                golds[1].color = defaultColor;
                golds[2].color = defaultColor;
                golds[0].material = noGold;
                golds[1].material = noGold;
                golds[2].material = noGold;
                break;
            case 1:
                // golds[0].enabled = true;
                // golds[1].enabled = false;
                // golds[2].enabled = false;
                golds[0].color = Color.white;
                golds[1].color = defaultColor;
                golds[2].color = defaultColor;
                golds[0].material = haveGold;
                golds[1].material = noGold;
                golds[2].material = noGold;
                
                break;
            case 2:
                // golds[0].enabled = true;
                // golds[1].enabled = true;
                // golds[2].enabled = false;
                golds[0].color = Color.white;
                golds[1].color = Color.white;
                golds[2].color = defaultColor;
                golds[0].material = haveGold;
                golds[1].material = haveGold;
                golds[2].material = noGold;
                break;
            case 3:
                // golds[0].enabled = true;
                // golds[1].enabled = true;
                // golds[2].enabled = true;
                golds[0].color = Color.white;
                golds[1].color = Color.white;
                golds[2].color = Color.white;
                golds[0].material = haveGold;
                golds[1].material = haveGold;
                golds[2].material = haveGold;
                break;
            default:
                break;
        }
    }

    public int getNumGoldBar()
    {
        return numGoldBar;
    }
}
