using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CustomerAnimator : MonoBehaviour
{
    [SerializeField] private CustomerObject customer;
    [SerializeField] private Rig customerRig;
    [SerializeField] private Rig customerEatingRig;
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_SITTING = "IsSitting";
    private const string IS_WAITINGFORSERVICE = "IsWaitingForService";
    private float customerHandsWeight;
    private float customerEatingHandWeight;
    public float speed = 3f; // Speed of the animation
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetBool(IS_WALKING, customer.IsWalking());
        animator.SetBool(IS_SITTING, customer.IsSitting());
        animator.SetBool(IS_WAITINGFORSERVICE, customer.IsWaitingForService());
        customerRig.weight = Mathf.Lerp(customerRig.weight, customerHandsWeight, Time.deltaTime * 10f);
        customerEatingRig.weight = Mathf.Lerp(customerEatingRig.weight, customerEatingHandWeight, Time.deltaTime * 10f);
        if (customer.IsEating())
        {
            customerHandsWeight = 1f;
            customerEatingHandWeight = Mathf.PingPong(Time.time, 1f);
        }
        else
        {
            customerHandsWeight = 0f;
            customerEatingHandWeight = 0f;
        }
    }
}
