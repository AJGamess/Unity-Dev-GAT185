using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Handles level-specific initialization such as player spawning and camera setup.
/// This component should be placed in each scene that requires these settings.
/// </summary>
public class GameLevel : MonoBehaviour
{
	[Header("Player Setup")]
	// The player prefab to instantiate when the scene starts
	[SerializeField] GameObject playerPrefab;

	// The initial spawn point for the player in this scene
	[SerializeField] Transform startPlayerSpawn;

	[Header("Camera")]
	// Cinemachine camera that will follow the player
	[SerializeField] CinemachineCamera playerCamera;

	[Header("Events")]
	// Event channel that notifies when a scene has finished loading
	[SerializeField] StringEvent sceneLoadedEvent;

	/// <summary>
	/// Subscribe to scene loaded event when this component is enabled
	/// </summary>
	private void OnEnable()
	{
		sceneLoadedEvent.Subscribe(OnSceneLoaded);
	}

	/// <summary>
	/// Unsubscribe from scene loaded event when this component is disabled
	/// </summary>
	private void OnDisable()
	{
		sceneLoadedEvent.Unsubscribe(OnSceneLoaded);
	}

	/// <summary>
	/// Called when a scene is loaded via the scene loading system
	/// </summary>
	/// <param name="sceneName">Name of the loaded scene</param>
	void OnSceneLoaded(string sceneName)
	{
		print("Scene loaded: " + sceneName);
	}

	/// <summary>
	/// Initialize the level when starting directly in the editor
	/// </summary>
	private void Start()
	{
		StartGame();
	}

	/// <summary>
	/// Initializes the level by spawning the player and setting up the camera
	/// </summary>
	void StartGame()
	{
		// Instantiate the player at the spawn point
		GameObject go = Instantiate(playerPrefab, startPlayerSpawn.position, startPlayerSpawn.rotation);

		// Configure Cinemachine camera to follow and look at the player
		playerCamera.Follow = go.transform;
		playerCamera.LookAt = go.transform;

		// Set up player's view reference to the camera
		if (go.TryGetComponent(out PlayerController controller))
		{
			print("Setting player view");
			controller.View = playerCamera.transform;
		}
	}
}