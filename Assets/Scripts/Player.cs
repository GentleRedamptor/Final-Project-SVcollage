using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    private GameObject muzzleflash;
    [SerializeField]
    private GameObject hitmarkerprefab;
    [SerializeField]
    private float ammo;
    [SerializeField]
    private float maxammo = 500;
    [SerializeField]
    private Animator weaponanim;
    [SerializeField]
    private bool canreload = false;
    private bool canshoot = true;
    public float coins = 0;
    [SerializeField]
    private GameObject futeristicAR;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        caculatemovement();
        if (Input.GetButton("Fire1") && ammo > 0 && canshoot == true && futeristicAR.activeInHierarchy)
        {
            canreload = false;
            shooting();
        }
        else if (Input.GetButtonUp("Fire1") && futeristicAR.activeInHierarchy)
        {
            canreload = true;
            muzzleflash.SetActive(false);
        }
        else
        {
            muzzleflash.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (Input.GetKeyDown(KeyCode.R) && canreload == true && futeristicAR.activeInHierarchy)
        {
            StartCoroutine(realod());
        }


    }

    void caculatemovement()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalinput, 0, verticalinput);
        Vector3 velocity = direction * speed;
        velocity = transform.TransformDirection(velocity);
        velocity.y -= gravity;
        controller.Move(velocity * Time.deltaTime);
    }

    void shooting()
    {
        muzzleflash.SetActive(true);
        Ray raycast = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitinfo;
        ammo -= 1;
        if (Physics.Raycast(raycast, out hitinfo, 100f))
        {
            Debug.Log("Hit:" + hitinfo.transform.name);
            GameObject hitanimation = Instantiate(hitmarkerprefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(hitanimation, 0.3f);


        }
        
    }

    public void addcoin(float i)
    {
        coins += i;
    }

    IEnumerator realod()
    {
        canreload = false;
        canshoot = false;
        weaponanim.SetTrigger("Reload");
        yield return new WaitForSeconds(1.6f);
        ammo = maxammo;
        canreload = true;
        canshoot = true;
    }

    public void activefuteristicAR()
    {
        futeristicAR.SetActive(true);
    }
}
