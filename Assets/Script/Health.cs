using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	[SyncVar (hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
	public bool destoryOnDeath;

	public RectTransform healthBar;

	private NetworkStartPosition[] spawnPoints;

	void Start () {
		if (isLocalPlayer) {
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}

	public void TakeDamage (int amount) {
		if (!isServer) return;

		currentHealth -= amount;
		if (currentHealth <= 0) {
			if (destoryOnDeath) {
				Destroy (gameObject);
			} else {
				currentHealth = maxHealth;
				RpcRespawn ();
			}
		}
	}

	void OnChangeHealth (int health) {
		healthBar.sizeDelta = new Vector2 (health, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn () {
		if (isLocalPlayer) {
			Vector3 spawnPoint = Vector3.zero;
			if (spawnPoints != null && spawnPoints.Length > 0) {
				spawnPoint = spawnPoints[Random.Range (0, spawnPoints.Length)].transform.position;
			}
			transform.position = spawnPoint;
		}
	}
}