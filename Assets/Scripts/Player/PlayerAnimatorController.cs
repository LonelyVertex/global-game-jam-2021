using Player.Controls;
using Player.State;
using UnityEngine;
using Zenject;

public class PlayerAnimatorController : MonoBehaviour
{
    [Inject]
    PlayerStateComponent playerState;
    [Inject]
    PlayerInputHandler playerInputHandler;
    
    [SerializeField]
    Animator animator;


    private const string kMovingString = "Speed";
    private const string kDrillingString = "Drilling";

    private int movingParamId;
    private int drillingParamId;
    
    void Start()
    {
        movingParamId = Animator.StringToHash(kMovingString);
        drillingParamId = Animator.StringToHash(kDrillingString);
    }

    void Update()
    {
        Debug.Log(playerState.IsDrilling);
        
        animator.SetFloat(movingParamId, playerInputHandler.IsMoving ? 1.0f : 0.0f);
        animator.SetBool(drillingParamId, playerState.IsDrilling);
    }
}
