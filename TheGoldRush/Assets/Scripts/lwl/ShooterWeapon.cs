using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecUnity;
using UnityEngine.UI;

public class ShooterWeapon : MonoBehaviour
{
    private float time;
    private PlayerBrain pb;
    public GameObject arrowPrefab;
    public Slider slider;
    public bool attacking;

    public AudioClip shoot;
    // public int shootingDir;
    // Start is called before the first frame update
    void Start()
    {
        time = Delay.shooter;
        pb = GetComponent<PlayerBrain>();
        slider.maxValue = Delay.shooter;
        slider.value = Delay.shooter;
        attacking = false;
    }

    private void Update()
    {
        if (time < Delay.shooter){
            slider.value = time;
            time += Time.deltaTime;
        }
        if (ParsecInput.GetKey(pb.PlayerID, KeyCode.J))
        {
            OnAttack();
        }
    }

    void OnAttack()
    {
        //if (GetComponent<Character>().chara != 1 || pb.stunned || pb.freezed || time < Delay.shooter)
        if (pb.stunned || pb.freezed || time < Delay.shooter)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(shoot, gameObject.transform.position, 0.7f);
        time = 0;
        Vector3 position = gameObject.transform.position;
        GameObject arrowInstance = GameObject.Instantiate(arrowPrefab);
        arrowInstance.GetComponent<Arrow>().color = pb.color;
        arrowInstance.GetComponent<Arrow>().master = this.gameObject;
        arrowInstance.GetComponent<Arrow>().ownerID = pb.PlayerID;
        Vector3 rotation = gameObject.transform.localEulerAngles;
        rotation.x = 0;
        rotation.y = 0;
        if (pb.dir == PlayerBrain.Direction.Down){
            rotation.z = 270;
            position.y -= 0.5f;
        }
        if (pb.dir == PlayerBrain.Direction.Up){
            rotation.z = 90;
            position.y += 0.5f;
        }
        if (pb.dir == PlayerBrain.Direction.Right){
            
            position.x += 0.5f;
        }
        if (pb.dir == PlayerBrain.Direction.Left){
            rotation.z = 180;
            position.x -= 0.5f;
        }
        
        // shootingDir = (int) pb.dir;
        attacking = true;

        arrowInstance.transform.position = position;
        arrowInstance.transform.localEulerAngles = rotation;
    }
}
