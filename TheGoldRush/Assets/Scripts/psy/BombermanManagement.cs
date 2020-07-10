using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecUnity;
using UnityEngine.UI;


public class BombermanManagement : MonoBehaviour
{
    public int MaxNumBombs = 3;
    public int BombSize = 1;
    public GameObject BombPrefab;
    public float DropBombCD = 0.2f;
    public AudioClip DropBombSoundClip;

    private PlayerBrain pb;
    public Slider slider;
    int NumBombs;
    float cd_timer;

    // Start is called before the first frame update
    void Start()
    {
        NumBombs = 0;
        pb = GetComponent<PlayerBrain>();
        cd_timer = DropBombCD;
        slider.maxValue = MaxNumBombs;
        slider.value = MaxNumBombs - NumBombs;
    }

    // Update is called once per frame
    void Update()
    {
        if (ParsecInput.GetKey(pb.PlayerID, KeyCode.J))
        {
            OnAttack();
        }
        if (cd_timer > 0)
        {
            cd_timer -= Time.deltaTime;
        }
        slider.value = MaxNumBombs - NumBombs;
    }

    void OnAttack(){
        //if (GetComponent<Character>().chara != 3 || pb.stunned || pb.freezed){
        if (pb.stunned || pb.freezed)
        {
            return;
        }
        DropBomb();
    }

    public void DropBomb()
    {
        if (NumBombs >= MaxNumBombs) 
            return;
        if (cd_timer > 0)
            return;

        Vector2 EnvPos = GameObject.Find("Env").transform.position;
        Vector2 GridPos = new Vector2(RoundToNearestGrid(transform.position.x - EnvPos.x) + EnvPos.x, RoundToNearestGrid(transform.position.y - EnvPos.y) + EnvPos.y);
        if (CheckOccupied(GridPos))
            return;

        AudioSource.PlayClipAtPoint(DropBombSoundClip, Camera.main.transform.position);
        GameObject go = Instantiate(BombPrefab, GridPos, Quaternion.identity);
        go.SendMessage("SetOwner", gameObject);
        go.SendMessage("SetBombSize", BombSize);
        NumBombs++;

        cd_timer = DropBombCD;
    }

    public void BombExplode()
    {
        NumBombs--;
    }

    float RoundToNearestGrid(float x)
    {
        return Mathf.Round(x - 0.5f) + 0.5f;
    }

    bool CheckOccupied(Vector2 pos)
    {
        Collider2D[] colls = Physics2D.OverlapPointAll(pos);
        foreach (Collider2D coll in colls)
        {
            if (coll.gameObject.CompareTag("bomb"))
            {
                return true;
            }
            if (coll.gameObject.CompareTag("box"))
            {
                return true;
            }
            if (coll.gameObject.CompareTag("Base"))
            {
                return true;
            }
            if (coll.gameObject.CompareTag("GoldMine"))
            {
                return true;
            }
        }
        return false;
    }
}
