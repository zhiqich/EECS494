using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecUnity;
using UnityEngine.UI;


public class PlayerWeapons : MonoBehaviour
{

    private PlayerBrain pb;

    public GameObject pickaxe;
    public GameObject playerWeapons;
    public Slider slider;

    public AudioClip swing;

    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBrain>();
        slider.maxValue = Delay.pickaxe;
        slider.value = Delay.pickaxe;
    }

    private float time = Delay.pickaxe;

    // Update is called once per frame
    void Update()
    {
        if (pb.freezed)
        {
            return;
        }

        if (pb.dir == PlayerBrain.Direction.Down)
        {
            playerWeapons.transform.up = Vector3.up;
        }
        else if (pb.dir == PlayerBrain.Direction.Up)
        {
            playerWeapons.transform.up = Vector3.down;
        }
        else if (pb.dir == PlayerBrain.Direction.Right)
        {
            playerWeapons.transform.up = Vector3.left;
        }
        else if (pb.dir == PlayerBrain.Direction.Left)
        {
            playerWeapons.transform.up = Vector3.right;
        }

        if (ParsecInput.GetKey(pb.PlayerID, KeyCode.J))
        {
            OnAttack();
        }
        if (time < Delay.pickaxe){
            slider.value = time;
            time += Time.deltaTime;
        }
    }

    void OnAttack()
    {
        // if (pb.stunned || pb.freezed || GetComponent<Character>().chara != 0)
        if (pb.stunned || pb.freezed || time < Delay.pickaxe)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(swing, gameObject.transform.position, 1f);
        time = 0;
        pickaxe.SetActive(true);
        pickaxe.GetComponent<Pickaxe>().Brandish();
    }
}
