/*

            Attatches the gun to the Hand gameobject.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A script that is put on the "StartWave" button. Attaches the gun to the hand object.
/// </summary>
public class Attach : MonoBehaviour
{
    /// <summary>
    /// The hand that the gun is put on.
    /// </summary>
    public Hand hand;
    public SteamVR_Action_Boolean changeAction;
    bool gunOn = false;
    GameObject assaultGun;
    GameObject handGun;

    void Awake()
    {
        assaultGun = GameObject.Find("AssaultGun").GetComponentInParent<Attach>().gameObject;
        handGun = GameObject.Find("HandGun").GetComponentInParent<Attach>().gameObject;

        if (gameObject == assaultGun)
        {
            handGun.SetActive(false);
        }
        else if (gameObject == handGun)
        {
            assaultGun.SetActive(false);
        }
    }

    private void Update()
    {
        if (gunOn == true)
        {
            Set();

            if (changeAction.GetStateDown(SteamVR_Input_Sources.Any))
            {
                ChangeGun();
            }
        }
    }

    public void EquipGun()
    {
        gunOn = true;
        gameObject.SetActive(true);
    }

    public void ChangeGun()
    {
        if (gameObject == assaultGun)
        {
            assaultGun.GetComponent<Attach>().UnSet();
            handGun.GetComponent<Attach>().EquipGun();
        }
        if (gameObject == handGun)
        {
            handGun.GetComponent<Attach>().UnSet();
            assaultGun.GetComponent<Attach>().EquipGun();
        }
    }

    /// <summary>
    /// Sets the gun to the hand. Spawns a new gun if no gun exists.
    /// </summary>
    public void Set()
    {
        hand.AttachObject(gameObject, GrabTypes.Grip, Hand.AttachmentFlags.ParentToHand);
        gameObject.transform.position = hand.transform.position;
        gameObject.transform.rotation = hand.transform.rotation;
    }

    /// <summary>
    /// Detaches the gun.
    /// </summary>
    public void UnSet()
    {
        if (hand.AttachedObjects != null)
        {
            hand.DetachObject(gameObject);
        }
        gunOn = false;
        gameObject.SetActive(false);
    }

    public void Reload()
    {
        assaultGun.GetComponentInChildren<GunShoot>().forceReloadGun();
        handGun.GetComponentInChildren<GunShoot>().forceReloadGun();
    }
}
