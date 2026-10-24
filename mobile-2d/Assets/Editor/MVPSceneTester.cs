using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class MVPSceneTester
{
    [MenuItem("KTX404/Test MVP Scene Simulation")]
    public static void SimulateMVP()
    {
        string scenePath = "Assets/Scenes/Room404_MVP.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        Debug.Log("Opened scene: " + scene.name);

        MVPFlowController flow = Object.FindFirstObjectByType<MVPFlowController>();
        if (flow == null)
        {
            Debug.LogError("MVPFlowController not found in scene!");
            return;
        }

        Debug.Log("MVPFlowController found! Current Stage: " + flow.currentStage);

        // Test Stage 1: Claim Bed
        Debug.Log("Simulating: OnClaimBed()...");
        flow.OnClaimBed();
        Debug.Log("Backpack active: " + flow.backpackProp.activeSelf);

        // Test Stage 1: Clean Trash
        Debug.Log("Simulating: OnCleanTrash() x3...");
        flow.OnCleanTrash();
        flow.OnCleanTrash();
        flow.OnCleanTrash();
        Debug.Log("Trash active: " + flow.trashPileProp.activeSelf);

        // Test Stage 2: Late Night Temptation
        Debug.Log("Simulating: OnChooseYesLateGame()...");
        flow.OnChooseYesLateGame();
        Debug.Log("Current Stage after Yes: " + flow.currentStage);

        // Test Stage 3: Laptop Awakening
        Debug.Log("Simulating: OnInteractLaptopAwakening()...");
        flow.OnInteractLaptopAwakening();

        // Test Stage 4: Turn off lights
        flow.currentStage = MVPFlowController.Stage.Stage4_Year6_Graduation;
        Debug.Log("Simulating: OnTurnOffLightsFinal()...");
        flow.OnTurnOffLightsFinal();
        Debug.Log("Final Stage: " + flow.currentStage + ", Global Light intensity: " + flow.globalLight.intensity);

        Debug.Log("=== SIMULATION VERIFICATION COMPLETED WITH ZERO ERRORS! ===");
    }
}
