using System.Reflection;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NailsmithBackPay;

public class NailsmithBackPay : Mod
{
    public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    public override void Initialize()
    {
        Log("Initializing");

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;

        Log("Initialized");
    }

    private void OnSceneChanged(Scene from, Scene to)
    {
        var sceneName = to.name;
        if (sceneName != "Ruins1_04") return;

        var ns = GameObject.Find("Nailsmith Cliff NPC");

        var prefabEde = GameObject.Find("Ruins Sentry 1").GetComponent<EnemyDeathEffects>();
        var copyToEde = ns.AddComponent<EnemyDeathEffects>();

        foreach (var field in prefabEde.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            field.SetValue(copyToEde, field.GetValue(prefabEde));
        copyToEde.SetAttr("corpse", (GameObject) null);
        copyToEde.SetAttr("corpsePrefab", (GameObject) null);
        copyToEde.SetAttr("audioPlayerPrefab", (AudioSource) null);
        copyToEde.SetAttr("deathWaveInfectedPrefab", new GameObject());
        copyToEde.SetAttr("spatterOrangePrefab", new GameObject());
        copyToEde.SetAttr("deathPuffMedPrefab", new GameObject());

        var prefabHm = GameObject.Find("Ruins Sentry 1").GetComponent<HealthManager>();
        var copyToHm = ns.AddComponent<HealthManager>();

        foreach (var field in prefabHm.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            field.SetValue(copyToHm, field.GetValue(prefabHm));
        copyToHm.SetAttr("hitEffectReceiver", (IHitEffectReciever) null);
        copyToHm.SetAttr("megaFlingGeo", true);
        copyToHm.hp = 1;
        copyToHm.SetGeoSmall(10);
        copyToHm.SetGeoMedium(3);
        copyToHm.SetGeoLarge(2);

        var fsm = ns.LocateMyFSM("Conversation Control");
        fsm.GetAction<PlayerDataBoolTest>("Idle", 0).boolName = "hasDash";
        fsm.GetAction<PlayerDataBoolTest>("Idle", 1).boolName = "";
        fsm.GetAction<PlayerDataBoolTest>("Idle", 2).boolName = "";
    }
}