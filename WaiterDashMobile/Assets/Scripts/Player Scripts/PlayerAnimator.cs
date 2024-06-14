using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimator : MonoBehaviour
{
    public static PlayerAnimator Instance;
    [SerializeField] private Rig rigRightHand;
    [SerializeField] private Player player;
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private float rightHandWeight;
    private bool isPlayerPickingItem;
    private bool isWashingDishes;
    private void Start()
    {
        Instance = this;
        player.OnPickedSomething += Player_OnPickedSomething;
        player.OnDroppedSomething += Player_OnDroppedSomething; ;
    }

    private void Player_OnDroppedSomething(object sender, System.EventArgs e)
    {
        isPlayerPickingItem = false;
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        isPlayerPickingItem = true;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        rigRightHand.weight = Mathf.Lerp(rigRightHand.weight, rightHandWeight, Time.deltaTime * 10f);
        if(isPlayerPickingItem)
        {
            rightHandWeight = 1f;
        }
        else
        {
            rightHandWeight = 0f;
        }
    }
}
