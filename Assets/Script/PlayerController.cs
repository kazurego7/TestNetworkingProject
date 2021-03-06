﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire ();
		}
	}

	// この [Command] コードはクライアントに呼び出されますが …
	// … サーバーで実行されます！
	[Command]
	void CmdFire () {
		// Bullet Prefab から Bullet を生成する
		var bullet = (GameObject) Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// 弾の速度を増加させる
		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;

		// Client 上に弾を生成する
		NetworkServer.Spawn (bullet);

		// 2 秒後に弾を破壊する
		Destroy (bullet, 2.0f);
	}

	public override void OnStartLocalPlayer () {
		GetComponent<MeshRenderer> ().material.color = Color.blue;
	}
}