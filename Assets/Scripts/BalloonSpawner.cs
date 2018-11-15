using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BalloonSpawner : MonoBehaviour {
	[SerializeField] private GameObject _balloonPrefab;
	
	[HideInInspector] public float secondsToGameOver;
	[HideInInspector] public float spawnSpeed;
	
	[HideInInspector] public float minimumBalloonSize;
	[HideInInspector] public float maximumBalloonSize;
	[HideInInspector] public float minimumBaseBalloonSpeed;
	[HideInInspector] public float maximumBaseBalloonSpeed;
	[HideInInspector] public float minimumBalloonSizeSpeed;
	[HideInInspector] public float maximumBalloonSizeSpeed;

	private float _spawnMostLeftPoint;
	private float _spawnMostRightPoint;
	private float _spawnBottomPoint;

	private Color[] colors = {
		Color.cyan,
		Color.red,
		Color.green, 
		Color.magenta,
		Color.yellow
	};

	void Start () {
		float heightHalfScreenSize = Camera.main.orthographicSize;
		float widthHalfScreenSize = heightHalfScreenSize * Camera.main.aspect;
		_spawnMostLeftPoint = -widthHalfScreenSize;
		_spawnMostRightPoint = widthHalfScreenSize;
		_spawnBottomPoint = -heightHalfScreenSize;
		
		StartCoroutine(SpawnBalloons());
	}
	
	void OnDestroy() {
		StopCoroutine(SpawnBalloons());
	}

	private IEnumerator SpawnBalloons() {
		float startTime = Time.time;
		while (secondsToGameOver - (Time.time - startTime) >= 0f) {
			yield return new WaitForSeconds(spawnSpeed);
			
			var balloonSize = Random.Range(minimumBalloonSize, maximumBalloonSize);
			var balloonSpeed = GetBalloonSpeed(balloonSize);
			var balloonPosition = new Vector2(
				Random.Range(_spawnMostLeftPoint+balloonSize/2, _spawnMostRightPoint-balloonSize/2),
				_spawnBottomPoint-balloonSize/2
				);
			var balloonObject = Instantiate(_balloonPrefab, balloonPosition, Quaternion.identity);
			var balloon = balloonObject.GetComponent<Balloon>();
			balloon.size = balloonSize;
			balloon.speed = balloonSpeed;
			balloon.SetColor(colors[Random.Range(0, colors.Length)]);
		}
	}

	private float GetBalloonSpeed(float balloonSize) {
		var baseSpeed = GetBaseBalloonSpeed();
		var specificSpeed = GetBalloonSpeedByBalloonSize(balloonSize);
		return baseSpeed + specificSpeed;
	}

	private float GetBaseBalloonSpeed() {
		var remainingTimeInPercent = Mathf.Clamp01(Time.time / secondsToGameOver);
		return Mathf.Lerp(minimumBaseBalloonSpeed, maximumBaseBalloonSpeed, remainingTimeInPercent);
	}

	private float GetBalloonSpeedByBalloonSize(float balloonSize) {
		var balloonSizeInPercent = balloonSize / (minimumBalloonSize + maximumBalloonSize);
		return Mathf.Lerp(minimumBalloonSizeSpeed, maximumBalloonSizeSpeed, 1-balloonSizeInPercent);
	}
	
}
