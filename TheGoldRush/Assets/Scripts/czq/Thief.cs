using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecUnity;
using UnityEngine.UI;

public class Thief : MonoBehaviour
{
    public LayerMask mask;
    bool active = true;
    bool dash = false;
    float dashDis = 2.0f;
    // Coroutine charge;
    PlayerBrain info;
    // PlayerInfo info;
    PlayerInventory inventory;
    public Slider slider;
    float timeToActive = 0;
    public GameObject ghost;
    public AudioClip teleport;

    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<PlayerBrain>();
        // info = GetComponent<PlayerInfo>();
        inventory = GetComponent<PlayerInventory>();
        slider.maxValue = 2;
        slider.value = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (ParsecInput.GetKey(info.PlayerID, KeyCode.J))
        {
            OnAttack();
        }
        if (!active)
        {
            timeToActive += Time.deltaTime;
            slider.value = timeToActive;
        }
    }

    void OnAttack()
    {
        // if (Input.GetKeyDown("joystick button 2"))
        // {
        //     charge = StartCoroutine(Charge());
        // }
        // if (Input.GetKeyUp("joystick button 2") && active == true)
        // {
        //     Debug.Log(dashDis);
        //     active = false;
        //     if (charge != null)
        //     {
        //         StopCoroutine(charge);
        //     }
        //     bool check = (Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis).collider == null);
        //     if (check == true)
        //     {
        //         transform.position += dashDis * info.DirectionVector[(int)info.dir];
        //     }
        //     else
        //     {
        //         transform.position += Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis).distance * info.DirectionVector[(int)info.dir];
        //     }
        //     StartCoroutine(CoolDown());
        // }
        if (info.stunned || info.freezed)
        {
            return;
        }
        //if (active == true && GetComponent<Character>().chara == 2)
        if (active == true)
        {
            // Debug.Log(dashDis);
            active = false;
            // if (charge != null)
            // {
            //     StopCoroutine(charge);
            // }
            timeToActive = 0;
            bool check = (Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis, mask).collider == null);
            Debug.Log(Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis, mask).collider);
            if (check == true)
            {
                // transform.position += dashDis * info.DirectionVector[(int)info.dir];
                StartCoroutine(MoveFromTo(transform.position, transform.position + dashDis * info.DirectionVector[(int)info.dir], 10.0f));
            }
            else
            {
                // transform.position += Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis).distance * info.DirectionVector[(int)info.dir];
                // StartCoroutine(MoveFromTo(transform.position, transform.position + (Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis).distance - 0.5f) * info.DirectionVector[(int)info.dir], 10.0f));
                // if (Physics2D.OverlapPoint(transform.position + dashDis * info.DirectionVector[(int)info.dir]) == null)
                if (Physics2D.OverlapBox(transform.position + dashDis * info.DirectionVector[(int)info.dir], new Vector2(0.4f, 0.4f), 0.0f) == null)
                {
                    // transform.position += dashDis * info.DirectionVector[(int)info.dir];
                    StartCoroutine(Opacity());
                    // StartCoroutine(MoveFromTo(transform.position, transform.position + dashDis * info.DirectionVector[(int)info.dir], 10.0f));
                }
                else
                {
                    StartCoroutine(MoveFromTo(transform.position, transform.position + (Physics2D.Raycast((Vector2)transform.position, (Vector2)info.DirectionVector[(int)info.dir], dashDis).distance - 0.5f) * info.DirectionVector[(int)info.dir], 10.0f));
                }
            }
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator Charge()
    {
        float currentTime = Time.time;
        while (Time.time - currentTime < 1.0f)
        {
            dashDis = 1.0f + Time.time - currentTime;
            yield return null;
        }
    }

    IEnumerator CoolDown()
    {
        // dashDis = 1.0f;
        yield return new WaitForSeconds(Delay.stealer);
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerTrigger" && other.GetComponent<PT>().player.GetComponent<PlayerBrain>().PlayerID != info.PlayerID && dash == true)
        {
            if (other.GetComponent<PT>().player.GetComponent<PlayerInventory>().numGoldBar > 0)
            {
                if (inventory.numGoldBar < info.capacity)
                {
                    // Debug.Log("...");
                    inventory.numGoldBar += 1;
                    inventory.PlayGetGoldMusic();
                    inventory.goldUpdate();
                    //inventory.numGoldBarText.text = inventory.numGoldBar.ToString();
                    other.GetComponent<PT>().player.GetComponent<PlayerInventory>().numGoldBar -= 1;
                    other.GetComponent<PT>().player.GetComponent<PlayerInventory>().goldUpdate();
                    //other.GetComponent<PT>().player.GetComponent<PlayerInventory>().numGoldBarText.text = other.GetComponent<PT>().player.GetComponent<PlayerInventory>().numGoldBar.ToString();
                }
            }
        }
        if (other.tag == "Base" && other.GetComponent<BaseTrigger>().color != info.color && dash == true)
        {
            Debug.Log("Outer");
            if (inventory.numGoldBar < info.capacity && other.GetComponent<BaseTrigger>().baseObject.GetComponent<Score>().GetScore() > 0)
            {
                Debug.Log("Inner");
                other.GetComponent<BaseTrigger>().baseObject.GetComponentInParent<Score>().PlayerScore(0, -1);
                inventory.numGoldBar += 1;
                inventory.PlayGetGoldMusic();
                inventory.goldUpdate();
            }
        }
    }

    IEnumerator MoveFromTo(Vector3 from, Vector3 to, float speed)
    {
        dash = true;
        AudioSource.PlayClipAtPoint(teleport, Camera.main.transform.position);
        float step = (speed / (from - to).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        float sb = 0.0f;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            transform.position = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
            if (t - sb >= 0.2f)
            {
                sb = t;
                GameObject shadow = Instantiate(ghost, transform.position, Quaternion.identity);
                shadow.GetComponent<Ghost>().dir = (int)info.dir;
            }
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        transform.position = to;
        dash = false;
    }

    IEnumerator Opacity()
    {
        float startTime = Time.time;
        float currentTime = Time.time;
        while (currentTime - startTime < 0.2f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (currentTime - startTime) / 0.2f);
            currentTime = Time.time;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        transform.position += dashDis * info.DirectionVector[(int)info.dir];
        startTime = Time.time;
        currentTime = Time.time;
        while (currentTime - startTime < 0.2f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, (currentTime - startTime) / 0.2f);
            currentTime = Time.time;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
