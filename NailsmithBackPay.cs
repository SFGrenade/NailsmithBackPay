using System.Reflection;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NailsmithBackPay
{
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

            var prefabEDE = GameObject.Find("Ruins Sentry 1").GetComponent<EnemyDeathEffects>();
            var copyToEDE = ns.AddComponent<EnemyDeathEffects>();

            foreach (var field in prefabEDE.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                field.SetValue(copyToEDE, field.GetValue(prefabEDE));
            copyToEDE.SetAttr("corpse", (GameObject) null);
            copyToEDE.SetAttr("corpsePrefab", (GameObject) null);
            copyToEDE.SetAttr("audioPlayerPrefab", (AudioSource) null);
            copyToEDE.SetAttr("deathWaveInfectedPrefab", new GameObject());
            copyToEDE.SetAttr("spatterOrangePrefab", new GameObject());
            copyToEDE.SetAttr("deathPuffMedPrefab", new GameObject());

            var prefabHM = GameObject.Find("Ruins Sentry 1").GetComponent<HealthManager>();
            var copyToHM = ns.AddComponent<HealthManager>();

            foreach (var field in prefabHM.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                field.SetValue(copyToHM, field.GetValue(prefabHM));
            copyToHM.SetAttr("hitEffectReceiver", (IHitEffectReciever) null);
            copyToHM.SetAttr("megaFlingGeo", true);
            copyToHM.hp = 1;
            copyToHM.SetGeoSmall(10);
            copyToHM.SetGeoMedium(3);
            copyToHM.SetGeoLarge(2);

            var fsm = ns.LocateMyFSM("Conversation Control");
            fsm.GetAction<PlayerDataBoolTest>("Idle", 0).boolName = "hasDash";
            fsm.GetAction<PlayerDataBoolTest>("Idle", 1).boolName = "";
            fsm.GetAction<PlayerDataBoolTest>("Idle", 2).boolName = "";
        }
    }
}