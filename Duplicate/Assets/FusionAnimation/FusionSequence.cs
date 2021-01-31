using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "FusionAnimationComponent", menuName = "Duplicate/FusionAnimationComponent", order = 0)]
public class FusionSequence : SerializedScriptableObject
{
    // Prefab References
    public GameObject PlayerGhostPrefab;

    public GameObject GlowNova_PS;
    public GameObject GodRays_PS;
    public GameObject Finalie_PS;

    [Button]
    public void TestPlay()
    {
        MainSequence().Play();
    }

    public Sequence MainSequence()
    {
        var levelTheme = DataManager.MakeItRain<LevelThemeData>(DataKeys.CURRENT_LEVEL_THEME);
        var playerTransforms = GetPlayerTransform();

        // Create new instance of player ghosts
        var playerA = GameObject.Instantiate(PlayerGhostPrefab, playerTransforms.A.position, Quaternion.identity);
        var playerB = GameObject.Instantiate(PlayerGhostPrefab, playerTransforms.B.position, Quaternion.identity);
        playerA.GetComponent<SpriteRenderer>().material = new Material(levelTheme.PlayerA_Material);
        playerB.GetComponent<SpriteRenderer>().material = new Material(levelTheme.PlayerB_Material);

        // Invert X Scale on playerB
        playerB.transform.localScale = new Vector3(-playerB.transform.localScale.x, playerB.transform.localScale.y, playerB.transform.localScale.z);

        Sequence seq = DOTween.Sequence();

        // Move back/up sequence
        seq.Append(MoveBackSequence(playerA, false));
        seq.Join(MoveBackSequence(playerB, true));

        // Hold in air for screen shake
        seq.Append(HoldInAirSequence(playerA, levelTheme, false));
        seq.Join(HoldInAirSequence(playerB, levelTheme, true));

        // Fly togeather sequence
        Vector2 endPosition = Vector2.Lerp(playerA.transform.position, playerB.transform.position, 0.5f);
        seq.Append(FlyTogetherSequence(playerA, endPosition));
        seq.Join(FlyTogetherSequence(playerB, endPosition));


        seq.AppendInterval(1.5f);
        seq.AppendCallback(() =>
            {
                GameObject.Instantiate(Finalie_PS, playerA.transform.position, Quaternion.identity);
                GameObject.Destroy(playerA);
                GameObject.Destroy(playerB);
            });

        return seq;
    }

    private (Transform A, Transform B) GetPlayerTransform()
    {
        // Get Player Transforms
        var PlayerA_transform = DataManager
                        .MakeItRain<PlayerController>(DataKeys.PLAYERA)
                        .GetComponentInChildren<SpriteRenderer>()
                        .transform;

        // Get Player Transforms
        var PlayerB_transform = DataManager
                        .MakeItRain<PlayerController>(DataKeys.PLAYERB)
                        .GetComponentInChildren<SpriteRenderer>()
                        .transform;

        return (PlayerA_transform, PlayerB_transform);
    }


    [FoldoutGroup("MoveBack Anim")] public Vector3 MoveBack_Offset = Vector3.left;
    [FoldoutGroup("MoveBack Anim")] public float MoveBack_Duration = 1f;
    [FoldoutGroup("MoveBack Anim")] public Ease MoveBack_Ease = Ease.OutQuad;
    private Sequence MoveBackSequence(GameObject player, bool inverted)
    {
        var seq = DOTween.Sequence();

        // Set x direction to move back appropriately
        var moveBackOffset = inverted
                            ? new Vector3(-MoveBack_Offset.x, MoveBack_Offset.y, 0)
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
    private Sequence HoldInAirSequence(GameObject player, LevelThemeData levelTheme, bool inverted)
    {
        var seq = DOTween.Sequence();

        // VFX
        seq.AppendCallback(() =>
        {
            // Create God Rays
            var godrays_ps = GameObject.Instantiate(GodRays_PS, player.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

            var col = inverted
                        ? levelTheme.PlayerB_VFX
                        : levelTheme.PlayerA_VFX;


            // Styleize God Rays VFX
            godrays_ps.startColor = new Color(col.r, col.g, col.b, godrays_ps.startColor.a);

            // Create Glow Nova, parent to player
            var nova_ps = GameObject.Instantiate(GlowNova_PS, player.transform.position, Quaternion.identity, player.transform).GetComponent<ParticleSystem>();

            // Styleize Glow Nova 
            nova_ps.startColor = new Color(col.r, col.g, col.b, nova_ps.startColor.a);
        });


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
    private Sequence FlyTogetherSequence(GameObject player, Vector2 endPosition)
    {
        var seq = DOTween.Sequence();

        // Set animation trigger
        var anim = player.GetComponent<Animator>();
        anim.SetTrigger("FlyTogeather");

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