using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] CustomerArrived;
    public AudioClip[] KitchenFoodReady;
    public AudioClip[] TrashThrown;
    public AudioClip[] PlayerPicked;
    public AudioClip[] PlayerThrown;
    public AudioClip[] PlayerWalk;
    public AudioClip[] PlayerTaskUnsuccessFull;
    public AudioClip[] PlayerTasksuccessFull;
    public AudioClip[] PlayerReserveCustomer;
    public AudioClip[] CustomerPayment;
    public AudioClip[] CustomerAskForHelp;
    public AudioClip[] CustomerEating;
}
