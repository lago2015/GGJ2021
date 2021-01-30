using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "FusionAnimationComponent", menuName = "Duplicate/FusionAnimationComponent", order = 0)]
public class FusionSequence : SerializedScriptableObject
{
    public GameObject Player1Image; // TODO find another way to get these references, can i use addressables?
    public GameObject Player2Image;

    [Button]
    public void TestPlay()
    {
        MainSequence().Play();
    }

    public Sequence MainSequence()
    {
        Player1Image = DataManager
                        .GetValue<PlayerController>(DataKeys.PLAYERA)
                        .GetComponentInChildren<SpriteRenderer>()
                        .gameObject;

        Player2Image = DataManager
                        .GetValue<PlayerController>(DataKeys.PLAYERB)
                        .GetComponentInChildren<SpriteRenderer>()
                        .gameObject;

        // Create new instance of player images
        var playerA = GameObject.Instantiate(Player1Image, Player1Image.transform.position, Quaternion.identity);
        var playerB = GameObject.Instantiate(Player2Image, Player2Image.transform.position, Quaternion.identity);

        // TODO create instances of particular prefab


        // Invert Sprite on playerB
        playerB.transform.localScale = new Vector3(-playerB.transform.localScale.x, playerB.transform.localScale.y, playerB.transform.localScale.z);

        Sequence seq = DOTween.Sequence();

        // Move back/up sequence
        seq.Append(MoveBackSequence(playerA, false));
        seq.Join(MoveBackSequence(playerB, true)); // Invert player 2.

        // Hold in air for screen shake
        seq.Append(HoldInAirSequence(playerA));
        seq.Join(HoldInAirSequence(playerB));

        // Fly togeather sequence
        Vector3 endPosition = Vector3.Lerp(playerA.transform.position, playerB.transform.position, 0.5f);
        seq.Append(FlyTogetherSequence(playerA, endPosition));
        seq.Join(FlyTogetherSequence(playerB, endPosition));

        // TODO Explosion VFX

        // TODO 

        seq.AppendInterval(0.5f);

        return seq;
    }

    [FoldoutGroup("MoveBack Anim")] public Vector3 MoveBack_Offset = Vector3.left;
    [FoldoutGroup("MoveBack Anim")] public float MoveBack_Duration = 1f;
    [FoldoutGroup("MoveBack Anim")] public Ease MoveBack_Ease = Ease.OutQuad;
    private Sequence MoveBackSequence(GameObject player, bool inverted)
    {
        var seq = DOTween.Sequence();

        // Set x direction to move back appropriately
        var moveBackOffset = inverted
                            ? new Vector3(-MoveBack_Offset.x, MoveBack_Offset.y, MoveBack_Offset.z)
                            : MoveBack_Offset;

        // Move robot back
        seq.Join(player.transform
            .DOMove(moveBackOffset, MoveBack_Duration)
            .SetRelative(true)
            .SetEase(MoveBack_Ease));
        return seq;
    }

    [FoldoutGroup("HoldInAir Anim")] public float HoldInAir_DelayBefore = 0.45f;
    [FoldoutGroup("HoldInAir Anim")] public float HoldInAir_Duration = 1f;
    [FoldoutGroup("HoldInAir Anim")] public float HoldInAir_Strength = 1.0f;
    [FoldoutGroup("HoldInAir Anim")] public int HoldInAir_Vabrato = 5;
    [FoldoutGroup("HoldInAir Anim")] public Ease HoldInAir_Ease = Ease.OutQuad;
    private Sequence HoldInAirSequence(GameObject player)
    {
        var seq = DOTween.Sequence();

        // Delay before
        seq.SetDelay(HoldInAir_DelayBefore);

        // Shake Robot in air
        seq.Join(player.transform
            .DOShakePosition(MoveBack_Duration, HoldInAir_Strength, HoldInAir_Vabrato, fadeOut: true)
            .SetEase(HoldInAir_Ease));

        return seq;
    }

    [FoldoutGroup("HoldInAir Anim")] public float FlyTogeather_Duration = 1f;
    [FoldoutGroup("HoldInAir Anim")] public Ease FlyTogether_Ease = Ease.OutQuad;
    [FoldoutGroup("HoldInAir Anim")] public float FlyTogether_MoveUp_Amount = 1f;
    [FoldoutGroup("HoldInAir Anim")] public Ease FlyTogether_MoveUp_Ease = Ease.OutQuad;
    private Sequence FlyTogetherSequence(GameObject player, Vector3 endPosition)
    {
        var seq = DOTween.Sequence();

        // Fly towards end position
        seq.Join(player.transform
            .DOMove(endPosition, FlyTogeather_Duration)
            .SetEase(MoveBack_Ease));

        seq.Join(player.transform
            .DOMoveY(FlyTogether_MoveUp_Amount, FlyTogeather_Duration)
            .SetRelative(true)
            .SetEase(FlyTogether_MoveUp_Ease));

        return seq;
    }
}